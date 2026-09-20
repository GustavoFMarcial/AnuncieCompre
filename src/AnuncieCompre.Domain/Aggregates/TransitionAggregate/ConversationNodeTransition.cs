using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.TransitionAggregate;

public class ConversationNodeTransition : BaseEntity
{
    public Guid ConversationNodeId { get; private set; }
    public ConversationNode ConversationNode { get; private set; } = default!;
    public string Key { get; private set; } = default!;
    public Guid TargetNodeId { get; private set; }

    private ConversationNodeTransition() {}

    private ConversationNodeTransition(ConversationNode conversationNode, string key, Guid targetNodeId)
    {
        ConversationNodeId = conversationNode.Id;
        ConversationNode = conversationNode;
        Key = key;
        TargetNodeId = targetNodeId;
    }

    public static Result<ConversationNodeTransition> Create(ConversationNode conversationNode, string key, Guid targetNodeId)
    {
        ConversationNodeTransition transition = new(conversationNode, key, targetNodeId);
        return Result<ConversationNodeTransition>.Success(transition, "Transition criado com sucesso");
    }
}