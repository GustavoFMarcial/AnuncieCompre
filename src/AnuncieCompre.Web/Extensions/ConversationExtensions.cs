using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.DTO;
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
            CustomerId = c.CustomerId,
            CustomerPhone = c.Customer.Phone.Value,
            CustomerName = c.Customer.Name?.Value ?? "",
            Status = c.Status.ToString().ToLower(),
            Attendant = c.Attendant.ToString().ToLower(),
            Messages = c.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType.ToString().ToLower(),
                Direction = m.Direction.ToString().ToLower(),
                IsRead = m.IsRead,
                CreatedAt = m.CreatedAt,
            }).ToList(),
        }).ToList();

        return new GetConversationsResponse
        {
            Conversations = conversations,
        };
    }

    public static ConversationDTO ToConversationDTO(this Conversation conversation)
    {
        return new ConversationDTO
        {
            ConversationId = conversation.Id,
            CustomerId = conversation.CustomerId,
            CustomerPhone = conversation.Customer.Phone.Value,
            CustomerName = conversation.Customer.Name?.Value ?? "",
            Status = conversation.Status.ToString().ToLower(),
            Attendant = conversation.Attendant.ToString().ToLower(),
            Messages = conversation.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType.ToString().ToLower(),
                Direction = m.Direction.ToString().ToLower(),
                IsRead = m.IsRead,
                CreatedAt = m.CreatedAt,
            }).ToList(), 
        };
    }

    public static RealTimeConversationDTO ToRealTimeConversationDTO(this Conversation conversation)
    {
        return new RealTimeConversationDTO
        {
            ConversationId = conversation.Id,
            Attendant = conversation.Attendant,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
        };
    }
}