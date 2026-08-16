using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditConversationFlowRequestExtensions
{
    public static EditConversationFlowInput ToConversationEditFlowInout(this EditConversationFlowRequest editFlowRequest)
    {
        return new EditConversationFlowInput
        {
            Name = editFlowRequest.Name,
            Description = editFlowRequest.Description,
            Status = editFlowRequest.Status switch
            {
                "Publicado" => FlowStatus.Published,
                "Rascunho" => FlowStatus.Draft,
                _ => FlowStatus.Draft,
            }
        };
    }
}