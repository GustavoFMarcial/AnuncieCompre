using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.Infra.Providers;

public class ConversationFlowProvider
{
    private Dictionary<string, ConversationNode> Flow { get; init; } = ConversationFlow.Build();

    public ConversationNode GetById(string? id)
    {
        if (id is null) return Flow["16"];

        if(!Flow.TryGetValue(id, out ConversationNode? conversationNode)) throw new ArgumentException("Id do ConversationNode inválido");

        return conversationNode;
    }
}