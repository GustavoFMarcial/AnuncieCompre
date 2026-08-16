using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases;

public class DeleteConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        flowRepository.Delete(flow);
        await unitOfWork.SaveChangesAsync();
        return Result.Success("ConversationFlow deletado com sucesso");
    }
}