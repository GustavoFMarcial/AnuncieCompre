using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork, MenuService _menuService)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly MenuService menuService = _menuService;

    public async Task<Result> Handle(Guid id, EditConversationFlowInput input)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        Result<Name> nameResult = Name.Create(input.Name);

        if (!nameResult.IsSuccess) return Result.Failure(nameResult.Message);

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            Result flowResult = flow.EditFlow(nameResult.Value, input.Description);

            if (!flowResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result.Failure(flowResult.Message);
            }

            await unitOfWork.SaveChangesAsync();
            Result menuResult = await menuService.UpdateMenuConversationNode();

            if (!menuResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result.Failure(flowResult.Message);
            }

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return Result.Success(flowResult.Message);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure(ex.Message);
        }
    }
}