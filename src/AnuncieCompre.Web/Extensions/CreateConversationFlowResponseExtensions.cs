using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CreateConversationFlowResponseExtensions
{
    public static CreateConversationFlowResponse ToCreateConversationFlowResponse(this Result<ConversationFlow> resultFlow)
    {
        return new CreateConversationFlowResponse
        {
            Id = resultFlow.Value.Id,
            Name = resultFlow.Value.Name.Value,
            Description = resultFlow.Value.Description,
            Status = resultFlow.Value.Status switch
            {
                FlowStatus.Published => "Publicado",
                FlowStatus.Draft => "Rascunho",
                _ => "Rascunho",
            },
            Steps = resultFlow.Value.Nodes?.Count > 0 ? resultFlow.Value.Nodes.Count : 0,
            CreatedAt = resultFlow.Value.CreatedAt,

        };
    }
}