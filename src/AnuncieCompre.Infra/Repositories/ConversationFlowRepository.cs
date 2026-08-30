using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationFlowRepository(AnuncieCompreContext _context) : BaseRepository<ConversationFlow>(_context), IConversationFlowRepository
{
    public async Task<List<ConversationFlow>> GetFlowsToListAsync()
    {
        return await context.Set<ConversationFlow>().OrderBy(cf => cf.CreatedAt).ToListAsync();
    }

    public async Task<ConversationFlow?> GetFlowByIdWithNodesAsync(Guid id)
    {
        return await context.Set<ConversationFlow>().Include(cf => cf.Nodes).FirstOrDefaultAsync(cf => cf.Id == id);
    }
}