using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Services.NodeValidatorFactories;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;

namespace AnuncieCompre.Application.UseCases.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(
    ICustomerRepository _customerRepository,
    IConversationRepository _conversationRepository,
    IConversationNodeRepository _conversationNodeRepository,
    IMessageRepository _messageRepository,
    IRealtimeNotifier _realmTimeNotifier,
    IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly IRealtimeNotifier realtimeNotifier = _realmTimeNotifier;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetOpenConversationBySenderPhoneAsync(incomingMessage.SenderPhone);
        bool isNewConversation = false;
        Message customerMessage;
        Collection<string> response = [];

        if (conversation is null)
        {
            Customer? customer = await customerRepository.GetCustomerByPhoneAsync(incomingMessage.SenderPhone);

            if (customer is null)
            {
                customer = Customer.Create(Phone.Create(incomingMessage.SenderPhone).Value);
                customerRepository.Add(customer);
            }

            conversation = Conversation.Create(customer);
            conversationRepository.Add(conversation);
            isNewConversation = true;
        }
        else if (conversation.Attendant == ConversationAttendant.Operator)
        {
            customerMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
            messageRepository.Add(customerMessage);

            await unitOfWork.SaveChangesAsync();
            await realtimeNotifier.NotifyNewMessageAsync(conversation.Id, customerMessage);
            return response.AsReadOnly();
        }

        ConversationNode awaitingResponseNode = await conversationNodeRepository.GetNodeWithTransitionsAndTargetNodesByIdAsync(conversation.AwaitingResponseNodeId);
        INodeValidator nodeValidator = NodeValidatorFactory.Generate(awaitingResponseNode);
        Result result = conversation.HandleMessage(nodeValidator, awaitingResponseNode, incomingMessage.Content);

        response.Add(result.Message);

        customerMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
        Message botMessage = Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing);
        messageRepository.Add(customerMessage);
        messageRepository.Add(botMessage);

        await unitOfWork.SaveChangesAsync();
        
        if (isNewConversation)
        {
            await realtimeNotifier.NotifyNewConversationAsync(conversation);
        }
        await realtimeNotifier.NotifyNewMessageAsync(conversation.Id, customerMessage);
        await realtimeNotifier.NotifyNewMessageAsync(conversation.Id, botMessage);
        return response.AsReadOnly();
    }
}