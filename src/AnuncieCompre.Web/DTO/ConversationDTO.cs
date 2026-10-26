namespace AnuncieCompre.Web.DTO;

public record ConversationDTO
{
    public Guid ConversationId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerPhone { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Attendant { get; set; } = default!;
    public List<MessageDTO> Messages { get; set; } = [];
}