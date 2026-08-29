namespace AnuncieCompre.Web.DTO;

public record ConversationDTO
{
    public Guid ConversationId { get; set; }
    public Guid UserId { get; set; }
    public string UserPhone { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Status { get; set; } = default!;
    public string Attendant { get; set; } = default!;
    public List<MessageDTO> Messages { get; set; } = [];
}