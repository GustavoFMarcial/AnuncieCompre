using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;

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

        flowRepository.Delete(flow);
        await unitOfWork.SaveChangesAsync();
        await menuService.UpdateMenuConversationNode();

        return Result.Success("ConversationFlow deletado com sucesso");
    }
}