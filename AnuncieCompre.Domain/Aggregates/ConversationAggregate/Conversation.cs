using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Phone UserPhone { get; private set; } = default!;
    public ConversationNode AwaitingResponseNode { get; private set; } = default!;
    public ConversationNode ActiveNode { get; private set; } = ConversationFlow.Build();
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

    public ReadOnlyCollection<string> HandleMessage(string message)
    {
        if (AwaitingResponseNode is null)
        {
            AwaitingResponseNode = ActiveNode;
            return [ActiveNode.Message];
        }

        return [message];
    }
}