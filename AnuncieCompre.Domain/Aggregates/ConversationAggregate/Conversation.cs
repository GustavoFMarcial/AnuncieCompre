using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Conversation.Nodes;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Phone UserPhone { get; private set; } = default!;

    private Conversation() {}

    private Conversation(Phone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(Phone userPhone)
    {
        return new Conversation(userPhone);
    }

    public (ReadOnlyCollection<string> response, string nextStepId) HandleMessage(IConversationNode awaitingResponseNode, string message, User user)
    {
        NodeResult result = awaitingResponseNode.NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess)
        {
            if (awaitingResponseNode.DomainEventFactory.Count > 0)
            {
                foreach (var domainEventFactory in awaitingResponseNode.DomainEventFactory)
                {
                    AddDomainEvent(domainEventFactory.Handle(user, result.Value));
                }
            }
        }

        return ([result.Message], result.NextStepId);
    }
}