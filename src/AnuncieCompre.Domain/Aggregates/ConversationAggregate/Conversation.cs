using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = default!;
    public Guid AwaitingResponseNodeId { get; private set; } = Guid.Empty;
    public DateTime DateTimeLastMessage { get; private set; }
    public ConversationAttendant Attendant { get; private set; } = ConversationAttendant.Bot;
    public ConversationStatus Status { get; private set; } = ConversationStatus.JustCreated;
    public DateTime EndedAt { get; private set; }
    public List<Message> Messages { get; private set; } = [];

    private Conversation() { }

    private Conversation(Customer customer)
    {
        CustomerId = customer.Id;
        Customer = customer;
    }

    public static Conversation Create(Customer user)
    {
        return new Conversation(user);
    }

    public Result HandleMessage(INodeValidator nodeValidator, ConversationNode awaitingResponseNode, string message)
    {
        DateTimeLastMessage = DateTime.UtcNow;

        if (Status == ConversationStatus.JustCreated)
        {
            Status = ConversationStatus.Open;
            AwaitingResponseNodeId = awaitingResponseNode.Id;
            return Result.Success("");
        }

        NodeResult result = nodeValidator.Validate(awaitingResponseNode, message);
        AwaitingResponseNodeId = result.NextStepId;

        if (result.IsSuccess)
        {
            return Result.Success("");
        }
        else
        {
            return Result.Failure(result.Message);
        }
    }

    public void Close()
    {
        EndedAt = DateTime.UtcNow;
        Status = ConversationStatus.Closed;
    }
}