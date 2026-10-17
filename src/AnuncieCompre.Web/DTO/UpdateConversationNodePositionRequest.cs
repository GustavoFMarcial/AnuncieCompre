using System.ComponentModel.DataAnnotations;

namespace AnuncieCompre.Web.DTO;

public record UpdateConversationNodePositionRequest
{
    [Required]
    public double Positionx { get; set; }
    [Required]
    public double PositionY { get; set; }
}