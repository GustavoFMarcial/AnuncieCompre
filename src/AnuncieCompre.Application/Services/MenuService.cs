using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Application.Services;

public class MenuService(IConversationFlowRepository _conversationFlowRepository, IConversationNodeRepository _conversationNodeRepository)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;

    public async Task UpdateMenuConversationNode()
    {
        ConversationNode? menuNode = await conversationNodeRepository.GetMenuConversationNodeAsync();
        List<ConversationNode> initialNodes = await conversationNodeRepository.GetInitialConversationNodesToListAsync();
        List<ConversationFlow> conversationFlows = await conversationFlowRepository.GetPublishedFlowsToListAsync();
        List<NodeTransition> nodeTransitions = [];
        string message = "Bem vindo, escolha uma opção para melhor te atender\n\n";
        List<string> options = [];

        for (int i = 0; i <= initialNodes.Count - 1; i++)
        {
            nodeTransitions.Add(NodeTransition.Create((i + 1).ToString(), initialNodes[i].Id).Value);
            options.Add((i + 1).ToString());
        }

        for (int i = 0; i <= conversationFlows.Count - 1; i++)
        {
            message += $"{i + 1} - {conversationFlows[i].Name.Value}\n";
        }

        if (menuNode is null)
        {
            ConversationFlow initialFlow = ConversationFlow.Create(Name.Create("InitialFlow").Value, "Menu de opções iniciais", FlowStatus.Published).Value;
            menuNode = ConversationNode.Create(initialFlow, message, ValidationKind.Option, true, nodeTransitions, options!).Value;
            conversationFlowRepository.Add(initialFlow);
            conversationNodeRepository.Add(menuNode);
        }

        menuNode.SetTransitions(nodeTransitions);
        menuNode.SetMessage(message);
        menuNode.SetOptions(options);
    }
}