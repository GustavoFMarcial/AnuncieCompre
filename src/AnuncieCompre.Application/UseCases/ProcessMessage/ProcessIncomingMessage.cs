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
using AnuncieCompre.Domain.Common;

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
        Conversation? conversation = await conversationRepository.GetOpenConversationBySenderPhoneAsync(incomingMessage.SenderPhone);
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
        else if (conversation.Attendant == ConversationAttendant.Operator)
        {
            messageRepository.Add(Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming));

            await unitOfWork.SaveChangesAsync();
            return response.AsReadOnly();
        }

        ConversationNode awaitingResponseNode = await conversationNodeRepository.GetNodeOrMenuWithTransitionsByIdAsync(conversation.AwaitingResponseNodeId);
        INodeValidator nodeValidator = NodeValidatorFactory.Generate(awaitingResponseNode);
        Result result = conversation.HandleMessage(nodeValidator, awaitingResponseNode, incomingMessage.Content);

        if (result.IsSuccess)
        {
            ConversationNode? targetNode = await conversationNodeRepository.GetByIdAsync(conversation.AwaitingResponseNodeId);

            if (targetNode is null)
            {
                response.Add("TargetNode não encontrado");
            }
            else
            {
                Result resultNextNode = conversation.HandleNextNode(targetNode);
                if (resultNextNode.IsSuccess)
                {
                    response.Add(resultNextNode.Message);
                }
                else
                {
                    response.Add(targetNode.Message);
                }
            }
        }
        else
        {
            response.Add(result.Message);
        }

        messageRepository.Add(Message.Create(conversation, incomingMessage.Content, MessageSenderType.Customer, MessageDirection.Incoming));
        messageRepository.Add(Message.Create(conversation, response[0], MessageSenderType.Bot, MessageDirection.Outgoing));

        await unitOfWork.SaveChangesAsync();
        return response.AsReadOnly();
    }
}