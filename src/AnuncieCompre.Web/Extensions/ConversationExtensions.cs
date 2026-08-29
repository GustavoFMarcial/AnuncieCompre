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
            UserId = c.UserId,
            UserPhone = c.User.Phone.Value,
            UserName = c.User.Name?.Value ?? "",
            Status = c.Status switch
            {
                ConversationStatus.Closed => "Encerrada",
                ConversationStatus.Open => "Aberta",
                ConversationStatus.JustCreated => "Nova",
                _ => "",
            },
            Attendant = c.Attendant switch
            {
                ConversationAttendant.Operator => "Operador",
                ConversationAttendant.Bot => "Bot",
                _ => "",
            },
            Messages = c.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType switch
                {
                    MessageSenderType.Bot => "Bot",
                    MessageSenderType.Operator => "Operador",
                    MessageSenderType.Customer => "Client",
                    _ => "",
                },
                Direction = m.Direction switch
                {
                    MessageDirection.Incoming => "Entrada",
                    MessageDirection.Outgoing => "Saída",
                    _ => "",
                },
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
            UserId = conversation.UserId,
            UserPhone = conversation.User.Phone.Value,
            UserName = conversation.User.Name?.Value ?? "",
            Status = conversation.Status switch
            {
                ConversationStatus.Closed => "Encerrada",
                ConversationStatus.Open => "Aberta",
                ConversationStatus.JustCreated => "Nova",
                _ => "",
            },
            Attendant = conversation.Attendant switch
            {
                ConversationAttendant.Operator => "Operador",
                ConversationAttendant.Bot => "Bot",
                _ => "",
            },
            Messages = conversation.Messages.Select(m => new MessageDTO
            {
                Text = m.Text,
                SenderType = m.SenderType switch
                {
                    MessageSenderType.Bot => "Bot",
                    MessageSenderType.Operator => "Operador",
                    MessageSenderType.Customer => "Client",
                    _ => "",
                },
                Direction = m.Direction switch
                {
                    MessageDirection.Incoming => "Entrada",
                    MessageDirection.Outgoing => "Saída",
                    _ => "",
                },
                CreatedAt = m.CreatedAt,
            }).ToList(), 
        };
    }
}