using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class ConversationExtensions
{
    public static GetConversationsResponse ToGetConversationsResponse(this List<Conversation> conversationList)
    {
        List<ConversationDTO> conversations = conversationList.Select(c => new ConversationDTO
        {
            ConversationId = c.Id,
            UserId = c.CustomerId,
            UserPhone = c.Customer.Phone.Value,
            UserName = c.Customer.Name?.Value ?? "",
            Status = c.Status.ToString().ToLower(),
            Attendant = c.Attendant.ToString().ToLower(),
            Messages = c.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType.ToString().ToLower(),
                Direction = m.Direction.ToString().ToLower(),
                CreatedAt = m.CreatedAt,
            }).ToList(),
        }).ToList();

        return new GetConversationsResponse
        {
            Conversations = conversations,
        };
    }

    public static ConversationDTO ToConversationDTO (this Conversation conversation)
    {
        return new ConversationDTO
        {
            ConversationId = conversation.Id,
            UserId = conversation.CustomerId,
            UserPhone = conversation.Customer.Phone.Value,
            UserName = conversation.Customer.Name?.Value ?? "",
            Status = conversation.Status.ToString().ToLower(),
            Attendant = conversation.Attendant.ToString().ToLower(),
            Messages = conversation.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType.ToString().ToLower(),
                Direction = m.Direction.ToString().ToLower(),
                CreatedAt = m.CreatedAt,
            }).ToList(), 
        };
    }
}