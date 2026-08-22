using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Domain.DTO;

public record TransitonInput
{
    public string? Option { get; set; }
    public Guid? TargetNodeId { get; set; }
}