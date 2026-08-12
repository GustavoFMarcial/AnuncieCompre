namespace AnuncieCompre.Web.DTO;

public record CreateFlowResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string Status { get; set; } = default!;
    public int Steps { get; set; }
    public DateTime CreatedAt { get; set; }
}