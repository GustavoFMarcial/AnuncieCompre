using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases;

public class DeleteConversationNode(IConversationFlowRepository _conversationFlowRepository, IConversationNodeRepository _conversationNodeRepository, IUnitOfWork _unitOfWork)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid flowId, Guid nodeId)
    {
        ConversationFlow? flow = await conversationFlowRepository.GetFlowByIdWithNodesWithTransitionsAsync(flowId);

        if (flow is null) return Result.Failure("ConversationFlow não encontrado");

        ConversationNode? node = flow.Nodes.FirstOrDefault(n => n.Id == nodeId);

        if (node is null) return Result.Failure("ConversationNode não encontrado");

        conversationNodeRepository.Delete(node);
        List<ConversationNode> nodesOriginTransition = flow.Nodes.Where(t => t.Transitions.Any(t => t.TargetNodeId == node.Id)).ToList();
        List<ConversationNode> nodesToRearrangeNumber = flow.Nodes.Where(n => n.Id != nodeId).OrderBy(n => n.Number).ToList();
        ConversationNode.RearrangeNumber(nodesToRearrangeNumber);

        foreach (ConversationNode n in nodesOriginTransition)
        {
            n.RemoveTransition(nodeId);
        }

        await unitOfWork.SaveChangesAsync();
        return Result.Success("ConversationNode deletado com sucesso");
    }
}