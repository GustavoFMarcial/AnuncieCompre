using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;

namespace AnuncieCompre.Application.UseCases.Flows;

public class GetConversationFlowById(IConversationFlowRepository _flowRepository)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;

    public async Task<ConversationFlow?> Handle(Guid id)
    {
        return await flowRepository.GetByIdAsync(id);
    }
}