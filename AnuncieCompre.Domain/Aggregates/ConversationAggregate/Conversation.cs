using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Phone UserPhone { get; private set; } = default!;
    public string AwaitingResponseNodeId { get; private set; } = default!;
    public Dictionary<string, ValueObject> TempData { get; private set; } = new();

    private Conversation() {}

    private Conversation(Phone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(Phone userPhone)
    {
        var conversation = new Conversation(userPhone);

        var domainEvent = new ConversationCreatedDomainEvent(conversation);
        conversation.AddDomainEvent(domainEvent);

        return conversation;
    }

    public ReadOnlyCollection<string> HandleMessage(string message, ConversationNode awaitingResponseNode)
    {
        if (AwaitingResponseNodeId is null)
        {
            AwaitingResponseNodeId = awaitingResponseNode.Id;
            return [awaitingResponseNode.Message];
        }

        NodeResult result = NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess)
        {
            AwaitingResponseNodeId = result.NextStepId!;

            if (awaitingResponseNode.TempDataType is not null)
            {
                TempData.Add(awaitingResponseNode.TempDataType, result.Value);
            }

            if (awaitingResponseNode.DomainEventFactory is not null)
            {
                var domainEvent = awaitingResponseNode.DomainEventFactory.Handle(UserPhone, TempData);
                AddDomainEvent(domainEvent);
                // TempData.Clear();
            }
        }

        return [result.Message];
    }
}