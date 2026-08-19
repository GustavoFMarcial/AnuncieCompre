using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationNodeTransitions(IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid nodeId, List<EditConversationNodeTransitionInput> input)
    {
        ConversationNode? node = await conversationNodeRepository.GetByIdAsync(nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        List<NodeTransition> transitions = [];

        foreach (EditConversationNodeTransitionInput i in input)
        {
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