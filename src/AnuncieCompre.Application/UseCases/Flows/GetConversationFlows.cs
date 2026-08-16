using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;

namespace AnuncieCompre.Application.UseCases;

public class GetConversationFlows(IConversationFlowRepository _flowRepository)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;

    public async Task<List<ConversationFlow>> Handle()
    {
        return await flowRepository.GetFlowsToListAsync();
    }
}