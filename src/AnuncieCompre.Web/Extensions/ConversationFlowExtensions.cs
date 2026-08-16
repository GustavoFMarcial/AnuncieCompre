using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class ConversationFlowExtensions
{
    public static GetConversationFlowByIdResponse ToGetConversationFlowByIdResponse(this ConversationFlow conversationFlow)
    {
        return new GetConversationFlowByIdResponse
        {
            Id = conversationFlow.Id,
            Name = conversationFlow.Name.Value,
            Description = conversationFlow.Description ?? "",
            Status = conversationFlow.Status switch
            {
                FlowStatus.Published => "Publicado",
                FlowStatus.Draft => "Rascunho",
                _ => "Rascunho",
            },
            Nodes = conversationFlow.Nodes.ToNodeDTO(),
        };
    }

    public static List<GetConversationFlowsResponse> ToGetConversationFlowsResponse(this List<ConversationFlow> conversationFlows)
    {
        return conversationFlows.Select(cf => new GetConversationFlowsResponse
        {
            Name = cf.Name.Value,
            Description = cf.Description,
            Status = cf.Status.ToString(),
        }).ToList();
    }
}