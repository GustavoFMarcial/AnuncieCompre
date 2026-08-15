using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class EditFlowRequestExtensions
{
    public static EditFlowInput ToEditFlowInout(this EditFlowRequest editFlowRequest)
    {
        return new EditFlowInput
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