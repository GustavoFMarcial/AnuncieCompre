using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases;

public class EditConversationNodeTransitions(IConversationFlowRepository _conversationFlowRepository, IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid flowId, Guid nodeId, EditConversationNodeTransitionInput input)
    {
        ConversationFlow? flow = await conversationFlowRepository.GetFlowWithNodesByIdAsync(flowId);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        ConversationNode? node = flow.Nodes.FirstOrDefault(n => n.Id == nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        List<ConversationNode> flowNodes = flow.Nodes;
        List<ConversationNodeTransition> transitions = [];

        foreach (Transiton t in input.Transitions)
        {
            ConversationNode? targetNode = flowNodes.FirstOrDefault(n => n.Id == t.TargetNodeId);
            if (targetNode is null) return Result.Failure("ConversationNode para transição não encontrado");
            Result<ConversationNodeTransition> result = ConversationNodeTransition.Create(node, t.Option, targetNode.Id, targetNode.Number);
            if (!result.IsSuccess) return Result.Failure(result.Message);
            transitions.Add(result.Value);
        }

        Result nodeResult = node.EditTransition(transitions, flowNodes);

        if (!nodeResult.IsSuccess) return Result.Failure(nodeResult.Message);

        await unitOfWork.SaveChangesAsync();
        return Result.Success(nodeResult.Message);
    }
}