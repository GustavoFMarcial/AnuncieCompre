using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class MessageExtensions
{
    public static RealTimeMessageDTO ToMessageDTO(this Message message)
    {
        return new RealTimeMessageDTO
        {
            ConversationId = message.ConversationId,
            SenderType = message.SenderType,
            Direction = message.Direction,
            CreatedAt = message.CreatedAt,
            Text = message.Text,
            IsRead = message.IsRead,
        };
    }

    public static List<RealTimeMarkAsReadMessageDTO> ToMarkAsReadMessageDto(this List<Message> messages)
    {
        return messages.Select(m => new RealTimeMarkAsReadMessageDTO
        {
            MessageId = m.Id,
            IsRead = m.IsRead,
        }).ToList();
        
    }
}