using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnuncieCompre.Application.UseCases;

public class DeleteConversationFlow(IConversationFlowRepository _flowRepository, IUnitOfWork _unitOfWork, MenuService _menuService)
{
    private readonly IConversationFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;
    private readonly MenuService menuService = _menuService;

    public async Task<Result> Handle(Guid id)
    {
        ConversationFlow? flow = await flowRepository.GetByIdAsync(id);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            flowRepository.Delete(flow);
            await unitOfWork.SaveChangesAsync();

            Result result = await menuService.UpdateMenuConversationNode();

            if (!result.IsSuccess)
            {
                await transaction.RollbackAsync();
                return Result.Failure(result.Message);
            }

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
            return Result.Success("ConversationFlow deletado com sucesso");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure(ex.Message);
        }
    }
}