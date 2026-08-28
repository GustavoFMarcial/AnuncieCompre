using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IConversationRepository : IBaseRepository<Conversation>
{
    public Task<Conversation?> GetOpenConversationByUserIdAsync(Guid userId);
    public Task<List<Conversation>> GetOpenConversationsAttendantByBotToListAsync();
    public Task<List<Conversation>> GetConversationsByStatusToListAsync(ConversationStatus? status);
}