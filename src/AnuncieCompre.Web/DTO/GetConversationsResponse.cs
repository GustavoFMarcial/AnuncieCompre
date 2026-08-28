namespace AnuncieCompre.Web.DTO;

public record GetConversationsResponse
{
    public List<ConversationDTO> Conversations { get; set; } = [];
}