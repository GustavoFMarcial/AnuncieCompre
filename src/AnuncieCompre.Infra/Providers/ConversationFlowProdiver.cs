using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Infra.Providers;

public class ConversationFlowProvider
{
    private readonly IReadOnlyDictionary<string, IConversationNode> InitialRegistration;

    public ConversationFlowProvider()
    {
        InitialRegistration = ConversationFlow.Build();
    }
    
    public IConversationNode GetById(string id)
    {
        if (id is null) return InitialRegistration["start"];
        IConversationNode? conversationNode;

        if (InitialRegistration.TryGetValue(id, out conversationNode)) return conversationNode;

        throw new KeyNotFoundException();
    }
}