using System.Collections.ObjectModel;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Domain.Conversation.Nodes;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate;

public class Conversation : BaseEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = default!;
    public string AwaitingResponseNodeId { get; private set; } = default!;
    public bool IsProcessing { get; private set; }
    public DateTime DateTimeLastMessage { get; private set; }
    public ConversationAttendant Attendant { get; private set; } = ConversationAttendant.Bot;
    public ConversationStatus Status { get; private set; } = ConversationStatus.JustCreated;
    public DateTime EndedAt { get; private set; }
    public List<Message> Messages { get; private set; } = [];

    private Conversation() { }

    private Conversation(User user)
    {
        UserId = user.Id;
        User = user;
    }

    public static Conversation Create(User user)
    {
        return new Conversation(user);
    }

    public ReadOnlyCollection<string> HandleMessage(IConversationNode awaitingResponseNode, string message, User user)
    {
        DateTimeLastMessage = DateTime.UtcNow;

        if (IsProcessing)
        {
            return ["Só um momento, ainda estamos processando sua última mensagem"];
        }

        IsProcessing = true;

        if (Status == ConversationStatus.JustCreated)
        {
            Status = ConversationStatus.Open;
            AwaitingResponseNodeId = awaitingResponseNode.Id;
            IsProcessing = false;
            return [awaitingResponseNode.Message];
        }

        if (awaitingResponseNode is FinalNode)
        {
            AwaitingResponseNodeId = awaitingResponseNode.Transitions["next"].Id;
            Status = ConversationStatus.Closed;
            IsProcessing = false;
            return [awaitingResponseNode.Transitions["next"].Message];
        }

        NodeResult result = awaitingResponseNode.NodeValidator.Validate(awaitingResponseNode, message);

        if (result.IsSuccess && result.ProcDomainEvent && awaitingResponseNode.DomainEventFactory.Count > 0)
        {
            foreach (var domainEventFactory in awaitingResponseNode.DomainEventFactory)
            {
                AddDomainEvent(domainEventFactory.Handle(user, result.Value));
            }
        }

        AwaitingResponseNodeId = result.NextStepId;
        IsProcessing = false;
        return [result.Message];
    }

    public void Close()
    {
        EndedAt = DateTime.Now;
        Status = ConversationStatus.Closed;
    }
}