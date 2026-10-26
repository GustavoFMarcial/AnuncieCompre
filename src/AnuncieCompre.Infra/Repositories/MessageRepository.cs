using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class MessageRepository(AnuncieCompreContext _context) : BaseRepository<Message>(_context), IMessageRepository
{
    public async Task<List<Message>> GetMessagesNotReadByConversationIdAsync(Guid conversationId)
    {
        return await context.Set<Message>()
            .Where(m => m.ConversationId == conversationId && m.SenderType == MessageSenderType.Customer && !m.IsRead).ToListAsync();
    }
}