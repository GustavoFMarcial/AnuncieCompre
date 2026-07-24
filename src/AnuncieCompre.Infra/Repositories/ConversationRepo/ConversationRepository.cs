using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.ConversationRepo;

public class ConversationRepository(AnuncieCompreContext _context) : BaseRepository<Conversation>(_context), IConversationRepository
{
    public async Task<Conversation?> GetOpenConversationByUserIdAsync(Guid userId)
    {
        return await context.Set<Conversation>().FirstOrDefaultAsync(c => c.UserId == userId && c.Status == ConversationStatus.Open);
    }
}