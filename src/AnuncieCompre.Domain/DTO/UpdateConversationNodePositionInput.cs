namespace AnuncieCompre.Domain.DTO;

public record UpdateConversationNodePositionInput
{
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}