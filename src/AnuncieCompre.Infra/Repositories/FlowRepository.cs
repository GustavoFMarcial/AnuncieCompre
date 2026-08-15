using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class FlowRepository(AnuncieCompreContext _context) : BaseRepository<ConversationFlow>(_context), IFlowRepository
{
    public async Task<List<ConversationFlow>> GetFlowsToListAsync()
    {
        return await context.Set<ConversationFlow>().ToListAsync();
    }
}