using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;

namespace AnuncieCompre.Application.UseCases;

public class GetFlows(IFlowRepository _flowRepository)
{
    private readonly IFlowRepository flowRepository = _flowRepository;

    public async Task<List<ConversationFlow>> Handle()
    {
        return await flowRepository.GetFlowsToListAsync();
    }
}