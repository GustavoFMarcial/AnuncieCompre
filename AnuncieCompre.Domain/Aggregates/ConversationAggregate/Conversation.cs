using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Phone UserPhone { get; private set; } = default!;
    public string AwaitingResponseNodeId { get; private set; } = default!;

    private Conversation() {}

    private Conversation(Phone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(Phone userPhone)
    {
        return new Conversation(userPhone);
    }

    public ReadOnlyCollection<string> HandleMessage(IConversationNode awaitingResponseNode, string message, User user)
    {
        if (AwaitingResponseNodeId is null)
        {
            AwaitingResponseNodeId = awaitingResponseNode.Id;
            return [awaitingResponseNode.Message];
        }

        NodeResult result = awaitingResponseNode.NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess)
        {
            AwaitingResponseNodeId = result.NextStepId!;

            if (awaitingResponseNode.DomainEventFactory.Count > 0)
            {
                foreach (var domainEventFactory in awaitingResponseNode.DomainEventFactory)
                {
                    AddDomainEvent(domainEventFactory.Handle(user, result.Value));
                }
            }
        }

        return [result.Message];
    }
}