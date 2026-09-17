using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class MessageProviderReferenceRepository(AnuncieCompreContext _context) : BaseRepository<MessageProviderReference>(_context), IMessageProviderReferenceRepository
{
    public async Task<MessageProviderReference?> GetMessageProviderReferenceWithMessageByMessageProviderSidAsync(string messageSid)
    {
        return await context.Set<MessageProviderReference>().Include(m => m.Message).FirstOrDefaultAsync(m => m.ProviderMessageId == messageSid);
    }
}