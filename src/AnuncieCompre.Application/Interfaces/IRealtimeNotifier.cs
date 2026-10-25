using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.Interfaces;

public interface IRealtimeNotifier
{
    Task NotifyNewMessageAsync(Guid conversationId, Message message);
}