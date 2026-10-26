using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;

namespace AnuncieCompre.Application.Interfaces;

public interface IRealtimeNotifier
{
    public Task NotifyNewMessageAsync(Guid conversationId, Message message);
    public Task NotifyNewConversationAsync(Conversation conversation);
}