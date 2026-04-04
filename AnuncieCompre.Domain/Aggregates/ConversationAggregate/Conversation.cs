using System.Collections.ObjectModel;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using System.Reflection;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Services;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public VOPhone UserPhone { get; private set; } = default!;
    public ConversationNode AwaitingResponseNode { get; private set; } = default!;
    public ConversationNode ActiveNode { get; private set; } = ConversationFlow.Build();
    public Dictionary<string, ValueObject> TempData { get; private set; } = new();

    private Conversation() {}

    private Conversation(VOPhone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(VOPhone userPhone)
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

        NodeResult result = ValidateMessage.Handle(AwaitingResponseNode, ActiveNode, message);

        if (result.IsSuccess)
        {
            AwaitingResponseNode = ActiveNode;
            ActiveNode = ActiveNode.Transitions[result.NextStep!];
        }

        return [result.Message];
    }
}