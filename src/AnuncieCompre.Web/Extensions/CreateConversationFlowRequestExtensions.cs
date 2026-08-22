using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Web.DTO;

public static class CreateConversationFlowRequestExtensions
{
    public static CreateConversationFlowInput ToCreateConversationFlowRequest(this CreateConversationFlowRequest request)
    {
        return new CreateConversationFlowInput
        {
            Name = request.Name,
            Description = request.Description,
            Status = request.Status.Trim().ToLower() switch
            {
                "publicado" => FlowStatus.Published,
                "rascunho" => FlowStatus.Draft,
                _ => FlowStatus.Draft,
            }
        };
    }
}