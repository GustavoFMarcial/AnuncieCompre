namespace AnuncieCompre.Web.DTO;

public record GetConversationFlowByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Status { get; set; } = default!;
    public List<Node> Nodes { get; set; } = [];
}