using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.TransitionAggregate;

public class ConversationNodeTransition : BaseEntity
{
    public Guid ConversationNodeId { get; private set; }
    public ConversationNode ConversationNode { get; private set; } = default!;
    public string Option { get; private set; } = default!;
    public Guid TargetNodeId { get; private set; }
    public ConversationNode TargetNode { get; private set; } = default!;
    public int TargetNodeNumber { get; private set; }

    private ConversationNodeTransition() {}

    private ConversationNodeTransition(ConversationNode conversationNode, string option, ConversationNode targetNode, int targetNodeNumber)
    {
        ConversationNodeId = conversationNode.Id;
        ConversationNode = conversationNode;
        Option = option;
        TargetNodeId = targetNode.Id;
        TargetNode = targetNode;
        TargetNodeNumber = targetNodeNumber;
    }

    public static Result<ConversationNodeTransition> Create(ConversationNode conversationNode, string option, ConversationNode targetNode, int TargetNodeNumber)
    {
        ConversationNodeTransition transition = new(conversationNode, option, targetNode, TargetNodeNumber);
        return Result<ConversationNodeTransition>.Success(transition, "Transition criado com sucesso");
    }
}