using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Providers;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Application.Dispatchers;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;

namespace AnuncieCompre.Application.UseCases.ProcessMessageUseCase;

public class ProcessIncomingMessageUseCase(
    ICustomerRepository _customerRepository,
    IConversationRepository _conversationRepository, 
    IMessageRepository _messageRepository, 
    ConversationFlowProvider _conversationFlowProvider, 
    EventDispatcher _dispatcher, 
    IUnitOfWork _unitOfWork) : IProcessIncomingMessage
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IConversationRepository conversationRepository = _conversationRepository;
    private readonly IMessageRepository messageRepository = _messageRepository;
    private readonly ConversationFlowProvider conversationFlowProvider = _conversationFlowProvider;
    private readonly EventDispatcher dispatcher = _dispatcher;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(incomingMessage.SenderPhone);
        Conversation? conversation;

        if (customer is null)
        {
            customer = Customer.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            conversation = Conversation.Create(customer);
            customerRepository.Add(customer);
            conversationRepository.Add(conversation);
        }

        conversation = await conversationRepository.GetOpenConversationByUserIdAsync(customer.Id);

        if (conversation is null)
        {
            conversation = Conversation.Create(customer);
            conversationRepository.Add(conversation);
        }

        IConversationNode awaitingRespondeNode = conversationFlowProvider.GetById(conversation.AwaitingResponseNodeId);

        ReadOnlyCollection<string> response = conversation.HandleMessage(awaitingRespondeNode, incomingMessage.Content, conversation.Customer);

        await dispatcher.DispatchAsync(conversation);
        
        Message userMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
        Message botMessage = Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing);
        messageRepository.Add(userMessage);
        messageRepository.Add(botMessage);

        await unitOfWork.SaveChangesAsync();
        return response;
    }
}