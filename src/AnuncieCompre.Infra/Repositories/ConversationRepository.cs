using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationRepository(AnuncieCompreContext _context) : BaseRepository<Conversation>(_context), IConversationRepository
{
    public async Task<Conversation?> GetOpenConversationByUserIdAsync(Guid userId)
    {
        return await context.Set<Conversation>().FirstOrDefaultAsync(c => c.CustomerId == userId && c.Status == ConversationStatus.Open);
    }

    public async Task<List<Conversation>> GetOpenConversationsAttendantByBotToListAsync()
    {
        return await context.Set<Conversation>().Where(c => c.Status == ConversationStatus.Open && c.Attendant == ConversationAttendant.Bot).ToListAsync();
    }

    public async Task<List<Conversation>> GetConversationsByStatusToListAsync(ConversationStatus? status)
    {
        IQueryable<Conversation> query = context.Set<Conversation>().Include(c => c.Messages.OrderBy(m => m.CreatedAt)).Include(c => c.Customer);

        if (status.HasValue)
        {
            query = query.Where(c => c.Status == status);
        }

        return await query.ToListAsync();
    }

    public async Task<Conversation?> GetConversationByIdWithMessagesAndUserAsync(Guid id)
    {
        return await context.Set<Conversation>().Include(c => c.Messages.OrderBy(m => m.CreatedAt)).Include(c => c.Customer).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Conversation?> GetConversationByIdWithUserAsync(Guid id)
    {
        return await context.Set<Conversation>().Include(c => c.Customer).FirstOrDefaultAsync(c => c.Id == id);
    }
}