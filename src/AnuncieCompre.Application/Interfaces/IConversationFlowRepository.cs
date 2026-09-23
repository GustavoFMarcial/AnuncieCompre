using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IConversationFlowRepository : IBaseRepository<ConversationFlow>
{
    public Task<List<ConversationFlow>> GetFlowsToListAsync();
    public Task<List<ConversationFlow>> GetPublishedFlowsToListAsync();
    public Task<ConversationFlow?> GetFlowByIdWithNodesAsync(Guid id);
    public Task<List<ConversationFlow>> GetFlowsWithNodesToListAsync();
    public Task<ConversationFlow?> GetFlowWithNodesByIdAsync(Guid id);
    public Task<List<ConversationFlow>> GetPublishedFlowsWithNodesAndMenuNodeToListAsync();
}