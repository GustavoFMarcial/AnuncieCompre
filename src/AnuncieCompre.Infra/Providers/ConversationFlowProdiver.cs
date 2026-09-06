using AnuncieCompre.Domain.Conversation.Flows;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Infra.Providers;

public class ConversationFlowProvider
{
    private readonly IReadOnlyDictionary<Guid, IConversationNode> InitialRegistration;

    public ConversationFlowProvider()
    {
        InitialRegistration = ConversationFlow.Build();
    }
    
    public IConversationNode GetById(Guid id)
    {
        IConversationNode? conversationNode;

        if (InitialRegistration.TryGetValue(id, out conversationNode)) return conversationNode;

        throw new KeyNotFoundException();
    }
}