using System.Collections.ObjectModel;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;

namespace AnuncieCompre.Application.Services;

public class ConversationFlowsMenu(IConversationFlowRepository _conversationFlowRepository)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;

    public async Task<ReadOnlyCollection<string>> CreateFlowsMenu()
    {
        List<ConversationFlow> flows = await conversationFlowRepository.GetFlowsToListAsync();

        Collection<string> menu = [];
        
        for (int i = 0; i >= flows.Count; i++)
        {
            menu.Add($"{i + 1} - {flows[i].Name}");
        }

        return menu.AsReadOnly();
    }
}