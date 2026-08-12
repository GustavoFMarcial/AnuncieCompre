using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Web.DTO;

public static class CreateFlowRequestExtensions
{
    public static CreateFlowInput ToCreateFlowRequest(this CreateFlowRequest createFlowInput)
    {
        return new CreateFlowInput
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