using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record TransitonRequest
{
    [Required]
    public string Option { get; set; } = default!;
    [Required]
    public Guid TargetNodeId { get; set; }
}