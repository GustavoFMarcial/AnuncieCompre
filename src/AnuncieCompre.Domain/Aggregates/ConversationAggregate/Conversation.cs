using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public Phone UserPhone { get; private set; } = default!;
    public string AwaitingResponseNodeId { get; private set; } = default!;
    public TimeOnly TimeLastMessage { get; private set; }
    public bool IsProcessing { get; private set; }
    public ConversationAttendant Attendant { get; private set; } = ConversationAttendant.Bot;
    public ConversationStatus Status { get; private set; } = ConversationStatus.Open;
    public List<Message> Messages { get; private set; } = [];
    public DateTime EndedAt { get; private set; }

    private Conversation() { }

    private Conversation(Phone phone)
    {
        UserPhone = phone;
    }

    public static Conversation Create(Phone userPhone)
    {
        return new Conversation(userPhone);
    }

    public (ReadOnlyCollection<string> response, string nextStepId) HandleMessage(IConversationNode awaitingResponseNode, string message, User user, bool isSessionJustCreated)
    {
        if (isSessionJustCreated)
        {
            return ([awaitingResponseNode.Message], awaitingResponseNode.Id);
        }

        if (awaitingResponseNode is FinalNode)
        {
            return ([awaitingResponseNode.Transitions["next"].Message], awaitingResponseNode.Transitions["next"].Id);
        }

        NodeResult result = awaitingResponseNode.NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess)
        {
            if (result.ProcDomainEvent)
            {
                if (awaitingResponseNode.DomainEventFactory.Count > 0)
                {
                    foreach (var domainEventFactory in awaitingResponseNode.DomainEventFactory)
                    {
                        AddDomainEvent(domainEventFactory.Handle(user, result.Value));
                    }
                }
            }
        }

        TimeLastMessage = TimeOnly.FromDateTime(DateTime.Now);
        return ([result.Message], result.NextStepId);
    }

    public string GetNodeIdByUserType(Enums.UserType userType)
    {
        return userType switch
        {
            Enums.UserType.Unknown => "initial_start",
            Enums.UserType.Customer => "ask_order",
            Enums.UserType.Vendor => "vendor_ask_premium",
            _ => "initial_start",
        };
    }
}