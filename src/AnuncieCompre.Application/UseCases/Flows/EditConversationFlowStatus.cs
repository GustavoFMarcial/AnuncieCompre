using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationFlowStatus(IConversationFlowRepository _conversationFlowRepository, IUnitOfWork _unitOfWork,  MenuService _menuService)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly MenuService menuService = _menuService;

    public async Task<Result> Handle(Guid flowId, EditConversationFlowStatusInput input)
    {
        ConversationFlow? flow = await conversationFlowRepository.GetFlowByIdWithNodesAsync(flowId);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        string errors = "";

        foreach (ConversationNode n in flow.Nodes)
        {
            Result nodeResult = n.ValidateTransitions(input.Status);

            if (!nodeResult.IsSuccess)
            {
                errors += $",{nodeResult.Message}";
            }
        }

        if (errors.Length > 0)
        {
            return Result.Failure(errors);
        }

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            Result flowResult = flow.EditStatus(input.Status);

            if (!flowResult.IsSuccess)
            {
                errors += flowResult.Message;
                await transaction.RollbackAsync();
            }

            await unitOfWork.SaveChangesAsync();
            Result menuResult = await menuService.UpdateMenuConversationNode();

            if (!menuResult.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result.Failure(menuResult.Message);
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