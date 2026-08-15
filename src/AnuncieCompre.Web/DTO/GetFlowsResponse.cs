namespace AnuncieCompre.Web.DTO;

public record GetFlowsResponse
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Status { get; set; } = default!;
}