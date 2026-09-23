using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Application.UseCases.Flows;

public class CreateConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork, MenuService _menuService)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly MenuService menuService = _menuService;

    public async Task<Result<ConversationFlow>> Handle(CreateConversationFlowInput input)
    {
        Result<Name> nameResult = Name.Create(input.Name);

        if (!nameResult.IsSuccess) return Result<ConversationFlow>.Failure(nameResult.Message);

        Result<ConversationFlow> flowResult = ConversationFlow.Create(nameResult.Value, input.Description, input.Status);

        if (!flowResult.IsSuccess) return flowResult;

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            flowRepository.Add(flowResult.Value);
            await unitOfWork.SaveChangesAsync();

            Result resultMenu = await menuService.UpdateMenuConversationNode();

            if (!resultMenu.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result<ConversationFlow>.Failure(resultMenu.Message);
            }

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return flowResult;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result<ConversationFlow>.Failure(ex.Message); 
        }
    }
}