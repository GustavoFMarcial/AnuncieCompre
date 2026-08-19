using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationFlowRequestExtensions
{
    public static EditConversationFlowInput ToConversationEditFlowInout(this EditConversationFlowRequest request)
    {
        return new EditConversationFlowInput
        {
            Name = request.Name,
            Description = request.Description,
            Status = request.Status switch
            {
                "Publicado" => FlowStatus.Published,
                "Rascunho" => FlowStatus.Draft,
                _ => FlowStatus.Draft,
            }
        };
    }
}