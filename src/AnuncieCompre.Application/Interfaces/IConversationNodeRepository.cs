using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Repositories;

namespace AnuncieCompre.Application.Interfaces;

public interface IConversationNodeRepository : IBaseRepository<ConversationNode>
{
    public Task<List<ConversationNode>> GetConversationNodeByTransitionTargetNodeIdAsync(Guid targetNodeId);
    public Task<List<Guid>> GetConversationNodesIdsByConversationFlowIdAsync(Guid flowId);
    public Task<List<ConversationNode>> GetConversationNodesByFlowIdAsync(Guid flowId);
    public Task<ConversationNode?> GetMenuConversationNodeAsync();
    public Task<List<ConversationNode>> GetInitialConversationNodesToListAsync();
    public Task<ConversationNode> GetNodeOrMenuByIdAsync(Guid id);
}