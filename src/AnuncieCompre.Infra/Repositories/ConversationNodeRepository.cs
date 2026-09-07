using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ConversationNodeRepository(AnuncieCompreContext _context) : BaseRepository<ConversationNode>(_context), IConversationNodeRepository
{
    public async Task<List<ConversationNode>> GetConversationNodeByTransitionTargetNodeIdAsync(Guid targetNodeId)
    {
        return await context.Set<ConversationNode>().Where(n => n.Transitions.Any(t => t.Value.Id == targetNodeId)).ToListAsync();
    }

    public async Task<List<ConversationNode>> GetConversationNodesByConversationFlowIdAsync(Guid flowId)
    {
        return await context.Set<ConversationNode>().Where(n => n.ConversationFlowId == flowId).ToListAsync();
    }

    public async Task<List<ConversationNode>> GetConversationNodesByFlowIdAsync(Guid flowId)
    {
        return await context.Set<ConversationNode>().Where(n => n.ConversationFlowId == flowId).ToListAsync();
    }

    public async Task<ConversationNode?> GetMenuConversationNodeAsync()
    {
        return await context.Set<ConversationNode>().FirstOrDefaultAsync(cn => cn.IsMenu == true);
    }

    public async Task<List<ConversationNode>> GetInitialConversationNodesToListAsync()
    {
        return await context.Set<ConversationNode>().Where(cn => cn.IsInitial == true).ToListAsync();
    }

    public async Task<ConversationNode> GetNodeOrMenuByIdAsync(Guid id)
    {
        ConversationNode? node = await context.Set<ConversationNode>().FirstOrDefaultAsync(cn => cn.Id == id);

        if (node is null)
        {
            return await context.Set<ConversationNode>().FirstAsync(cn => cn.IsMenu == true);
        }

        return node;
    }
}