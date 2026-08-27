using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id, EditConversationFlowInput input)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        Result<Name> nameResult = Name.Create(input.Name);

        if (!nameResult.IsSuccess) return Result.Failure(nameResult.Message);

        Result result = flow.EditFlow(nameResult.Value, input.Description);

        if (!result.IsSuccess) return Result.Failure(result.Message);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(result.Message);
    }
}