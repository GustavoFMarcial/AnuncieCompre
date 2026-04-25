using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Flows;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.Nodes;

namespace AnuncieCompre.Infra.Providers;

public class ConversationFlowProvider
{
    private Dictionary<string, ConversationNode> InitialRegistration { get; init; } = InitialRegistrationFlow.Build();
    private Dictionary<string, ConversationNode> Customer { get; init; } = CustomerFlow.Build();
    private Dictionary<string, ConversationNode> Vendor { get; init; } = VendorFlow.Build();

    public ConversationNode GetById(string? id)
    {
        if (id is null) return InitialRegistration["initial_start"];
        ConversationNode? conversationNode;

        if (InitialRegistration.TryGetValue(id, out conversationNode)) return conversationNode;
        if (Customer.TryGetValue(id, out conversationNode)) return conversationNode;
        if (Vendor.TryGetValue(id, out conversationNode)) return conversationNode;

        throw new KeyNotFoundException();
    }
}