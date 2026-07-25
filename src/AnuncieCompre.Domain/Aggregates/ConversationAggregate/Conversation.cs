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
    public ConversationStatus Status { get; private set; } = ConversationStatus.JustCreated;
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

    public ReadOnlyCollection<string> HandleMessage(IConversationNode awaitingResponseNode, string message, User user)
    {
        TimeLastMessage = TimeOnly.FromDateTime(DateTime.Now);

        if (Status == ConversationStatus.JustCreated)
        {
            Status = ConversationStatus.Open;
            AwaitingResponseNodeId =  awaitingResponseNode.Id;
            return [awaitingResponseNode.Message];
        }

        if (awaitingResponseNode is FinalNode)
        {
            AwaitingResponseNodeId = awaitingResponseNode.Transitions["next"].Id;
            return [awaitingResponseNode.Transitions["next"].Message];
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

        AwaitingResponseNodeId = result.NextStepId;
        return [result.Message];
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

    public void Close()
    {
        Status = ConversationStatus.Closed;
    }
}