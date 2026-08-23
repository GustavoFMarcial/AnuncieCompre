using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationNodeRepository(AnuncieCompreContext _context) : BaseRepository<ConversationNode>(_context), IConversationNodeRepository
{
    public async Task<List<ConversationNode>> GetConversationNodeByTransitionTargetNodeId(Guid targetNodeId)
    {
        return await context.Set<ConversationNode>().Where(n => n.Transitions.Any(t => t.TargetNodeId == targetNodeId)).ToListAsync();
    }

    public async Task<List<Guid>> GetConversationNodesIdsByConversationFlowId(Guid flowId)
    {
        return await context.Set<ConversationNode>().Where(n => n.ConversationFlowId == flowId).Select(n => n.Id).ToListAsync();
    }
}