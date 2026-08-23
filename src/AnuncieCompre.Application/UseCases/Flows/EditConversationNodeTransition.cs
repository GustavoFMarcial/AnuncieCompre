using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationNodeTransitions(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid nodeId, EditConversationNodeTransitionInput input)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        List<Guid> flowNodesIds = await conversationNodeRepository.GetConversationNodesIdsByConversationFlowId(node.ConversationFlowId);
        List<NodeTransition> transitions = [];

        foreach (TransitonInput i in input.Transitions)
        {
            if (!flowNodesIds.Exists(n => n == i.TargetNodeId)) return Result.Failure("TargetNodeId não encontrado no ConversationFlow");
            Result<NodeTransition> transitionResult = NodeTransition.Create(i.Option, i.TargetNodeId);

            if (!transitionResult.IsSuccess) return Result.Failure(transitionResult.Message);
            transitions.Add(transitionResult.Value);
        }

        Result nodeResult = node.EditTransition(transitions);

        if (!nodeResult.IsSuccess) return Result.Failure(nodeResult.Message);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(nodeResult.Message);
    }
}