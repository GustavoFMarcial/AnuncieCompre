using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = default!;
    public string AwaitingResponseNodeId { get; private set; } = "start";
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

    public ReadOnlyCollection<string> HandleMessage(IConversationNode awaitingResponseNode, string message, Customer customer)
    {
        DateTimeLastMessage = DateTime.UtcNow;

        if (Status == ConversationStatus.JustCreated)
        {
            Status = ConversationStatus.Open;
            AwaitingResponseNodeId = awaitingResponseNode.Id;
            return [awaitingResponseNode.Message];
        }

        NodeResult result = awaitingResponseNode.NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess && result.ProcDomainEvent && awaitingResponseNode.DomainEventFactory.Count > 0)
        {
            foreach (var domainEventFactory in awaitingResponseNode.DomainEventFactory)
            {
                AddDomainEvent(domainEventFactory.Handle(customer, result.Value));
            }
        }

        AwaitingResponseNodeId = result.NextStepId;
        return [result.Message];
    }

    public void Close()
    {
        EndedAt = DateTime.UtcNow;
        Status = ConversationStatus.Closed;
    }
}