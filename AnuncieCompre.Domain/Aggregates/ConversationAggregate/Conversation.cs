using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Aggregates.UserAggregate;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public VOPhone UserPhone { get; private set; } = default!;
    public ConversationNode ConversationNode { get; private set; } = ConversationFlow.Build();
    public Dictionary<string, ValueObject> TempDataa { get; private set; } = new();

    private Conversation() {}

    private Conversation(VOPhone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(VOPhone userPhone)
    {
        return new Conversation(userPhone);
    }

    public ReadOnlyCollection<string> HandleMessage(IncomingMessageRequest message)
    {
        if (ConversationNode.HasValidation)
        {
            
        }

        return ["a"];
    }
}