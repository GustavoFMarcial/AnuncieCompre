using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Web.DTO;

public static class CreateFlowInputExtensions
{
    extension(CreateFlowInput createFlowInput)
    {
        public CreateFlowRequest ToCreateFlowRequest()
        {
            return new CreateFlowRequest
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
}