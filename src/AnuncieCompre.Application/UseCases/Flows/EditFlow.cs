using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditFlow(IFlowRepository _flowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id, EditFlowInput input)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result.Failure("Flow não encontrado");

        Result result = flow.EditFlow(input);

        if (!result.IsSuccess) return Result.Failure(result.Message);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(result.Message);
    }
}