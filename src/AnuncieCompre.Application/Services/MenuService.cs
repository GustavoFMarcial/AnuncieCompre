using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.TransitionAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Application.Services;

public class MenuService(IConversationFlowRepository _conversationFlowRepository, IConversationNodeRepository _conversationNodeRepository)
{
    private readonly IConversationFlowRepository conversationFlowRepository = _conversationFlowRepository;
    private readonly IConversationNodeRepository conversationNodeRepository = _conversationNodeRepository;

    public async Task UpdateMenuConversationNode()
    {
        List<ConversationFlow> conversationFlows = await conversationFlowRepository.GetPublishedFlowsWithNodesToListAsync();
        List<ConversationNode> initialNodes = conversationFlows.SelectMany(cf => cf.Nodes.Where(n => n.IsInitial)).ToList();
        ConversationNode? menuNode = conversationFlows.SelectMany(cf => cf.Nodes).FirstOrDefault(n => n.IsMenu);
        List<ConversationNodeTransition> nodeTransitions = [];
        string message = "Bem vindo, escolha uma opção para melhor te atender\n\n";
        List<string> options = [];

        if (menuNode is null)
        {
            ConversationFlow initialFlow = ConversationFlow.Create(Name.Create("InitialFlow").Value, "Menu de opções iniciais", FlowStatus.Published, true).Value;
            menuNode = ConversationNode.Create(initialFlow, message, ValidationKind.Option, true, nodeTransitions, options!).Value;
            conversationFlowRepository.Add(initialFlow);
            conversationNodeRepository.Add(menuNode);
        }

        for (int i = 0; i <= conversationFlows.Count - 1; i++)
        {
            message += $"{i + 1} - {conversationFlows[i].Name.Value}\n";
        }

        for (int i = 0; i <= initialNodes.Count - 1; i++)
        {
            Result<ConversationNodeTransition> result = ConversationNodeTransition.Create(menuNode, (i + 1).ToString(), initialNodes[i].Id);
            if (!result.IsSuccess) return;
            nodeTransitions.Add(result.Value);
            options.Add((i + 1).ToString());
        }

        menuNode.SetTransitions(nodeTransitions);
        menuNode.SetMessage(message);
        menuNode.SetOptions(options);
    }
}