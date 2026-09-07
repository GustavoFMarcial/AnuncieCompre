using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationNodeTransitions(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid nodeId, EditConversationNodeTransitionInput input)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        List<ConversationNode> flowNodes = await conversationNodeRepository.GetConversationNodesByFlowIdAsync(node.ConversationFlowId);
        Result nodeResult = node.EditTransition(input.Transitions, flowNodes);

        if (!nodeResult.IsSuccess) return Result.Failure(nodeResult.Message);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(nodeResult.Message);
    }
}