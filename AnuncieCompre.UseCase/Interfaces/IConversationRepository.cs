using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IConversationRepository : IBaseRepository<Conversation>
{
    public Task<Conversation?> GetConversationByPhoneAsync(string userPhone);
}