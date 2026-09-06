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
using AnuncieCompre.Domain.Services.ValueObjectFactories;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Infra.Repositories;
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
    // private readonly EventDispatcher dispatcher = _dispatcher;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<ReadOnlyCollection<string>> ExecuteAsync(IncomingMessageRequest incomingMessage)
    {
        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(incomingMessage.SenderPhone);
        Conversation? conversation;
        Collection<string> response = [];

        if (customer is null)
        {
            customer = Customer.Create(Phone.Create(incomingMessage.SenderPhone).Value);
            customerRepository.Add(customer);
        }

        conversation = await conversationRepository.GetOpenConversationByUserIdAsync(customer.Id);

        if (conversation is null)
        {
            conversation = Conversation.Create(customer);
            conversationRepository.Add(conversation);
        }

        ConversationNode awaitingResponseNode = await conversationNodeRepository.GetNodeOrMenuByIdAsync(conversation.AwaitingResponseNodeId);
        INodeValidator nodeValidator = NodeValidatorFactory.Handle(awaitingResponseNode);
        NodeResult result = nodeValidator.Validate(awaitingResponseNode, incomingMessage.Content);
        response.Add(result.Message);

        conversation.UpdateAwaitingResponseNodeId(result.NextStepId);

        // await dispatcher.DispatchAsync(conversation);
        
        Message userMessage = Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming);
        Message botMessage = Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing);
        messageRepository.Add(userMessage);
        messageRepository.Add(botMessage);

        await unitOfWork.SaveChangesAsync();
        return response.AsReadOnly();
    }
}