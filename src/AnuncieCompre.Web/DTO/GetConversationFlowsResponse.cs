namespace AnuncieCompre.Web.DTO;

public record GetConversationFlowsResponse
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Status { get; set; } = default!;
}