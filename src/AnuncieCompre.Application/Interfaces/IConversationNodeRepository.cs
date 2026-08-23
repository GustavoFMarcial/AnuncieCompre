using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.Infra.Repositories;

namespace AnuncieCompre.Application.Interfaces;

public interface IConversationNodeRepository : IBaseRepository<ConversationNode>
{
    public Task<List<ConversationNode>> GetConversationNodeByTransitionTargetNodeId(Guid targetNodeId);
    public Task<List<Guid>> GetConversationNodesIdsByConversationFlowId(Guid flowId);
}