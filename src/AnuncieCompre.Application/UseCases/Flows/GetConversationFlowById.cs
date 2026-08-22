using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases.Flows;

public class GetConversationFlowById(IConversationFlowRepository _flowRepository)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;

    public async Task<Result<ConversationFlow>> Handle(Guid id)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result<ConversationFlow>.Failure("ConversationFlow não encontrado");

        return Result<ConversationFlow>.Success(flow, "ConversationFlow encontrado com sucesso");
    }
}