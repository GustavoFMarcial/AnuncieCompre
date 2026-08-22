using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record TransitonRequest
{
    public string? Option { get; set; }
    public Guid? TargetNodeId { get; set; }
}