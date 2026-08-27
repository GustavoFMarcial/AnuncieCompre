using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IConversationFlowRepository : IBaseRepository<ConversationFlow>
{
    public Task<List<ConversationFlow>> GetFlowsToListAsync();
    public Task<ConversationFlow?> GetFlowByIdWithNodesAsync(Guid id);
}