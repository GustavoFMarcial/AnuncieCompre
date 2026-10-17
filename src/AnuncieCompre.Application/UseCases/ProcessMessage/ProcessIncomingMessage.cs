using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Services.NodeValidatorFactories;
using AnuncieCompre.Domain.Conversation.Nodes;

namespace AnuncieCompre.Application.UseCases.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(
    ICustomerRepository _customerRepository,
    IConversationRepository _conversationRepository,
    IConversationNodeRepository _conversationNodeRepository,
    IMessageRepository _messageRepository,
    // EventDispatcher _dispatcher, 
    IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Conversation? conversation = await conversationRepository.GetNotClosedConversationBySenderPhoneWithCustomerAsync(incomingMessage.SenderPhone);
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
        }

        ConversationNode awaitingResponseNode = await conversationNodeRepository.GetNodeOrMenuWithTransitionsByIdAsync(conversation.AwaitingResponseNodeId);
        INodeValidator nodeValidator = NodeValidatorFactory.Generate(awaitingResponseNode);
        (bool conversationResult, string message) = conversation.HandleMessage(nodeValidator, awaitingResponseNode, incomingMessage.Content);

        if (conversationResult)
        {
            ConversationNode? targetNode = await conversationNodeRepository.GetByIdAsync(conversation.AwaitingResponseNodeId);

            if (targetNode is null)
            {
                response.Add("TargetNode não encontrado");
                return response.AsReadOnly();
            }
            
            response.Add(targetNode.Message);
        }
        else
        {
            response.Add(message);
        }

        Message customerMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
        Message botMessage = Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing);
        messageRepository.Add(customerMessage);
        messageRepository.Add(botMessage);

        await unitOfWork.SaveChangesAsync();
        return response.AsReadOnly();
    }
}