using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Web.DTO;

public static class CreateConversationFlowRequestExtensions
{
    public static CreateConversationFlowInput ToCreateConversationFlowRequest(this CreateConversationFlowRequest createFlowInput)
    {
        return new CreateConversationFlowInput
        {
            Name = createFlowInput.Name,
            Description = createFlowInput.Description,
            Status = createFlowInput.Status.Trim().ToLower() switch
            {
                "Publicado" => FlowStatus.Published,
                "Rascunho" => FlowStatus.Draft,
                _ => FlowStatus.Draft,
            }
        };
    }
}