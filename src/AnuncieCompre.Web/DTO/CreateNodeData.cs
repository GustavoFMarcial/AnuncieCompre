using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record CreateNodeData
{
    [Required]
    public string Text { get; set; } = default!;
    public string? Validation { get; set; }
}