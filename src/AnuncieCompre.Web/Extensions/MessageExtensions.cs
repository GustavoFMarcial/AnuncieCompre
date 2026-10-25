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
        };
    }
}