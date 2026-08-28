using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class ConversationExtensions
{
    public static GetConversationsResponse ToGetConversationsResponse(this List<Conversation> response)
    {
        List<ConversationDTO> conversations = response.Select(c => new ConversationDTO
        {
            ConversationId = c.Id,
            UserId = c.UserId,
            UserName = c.User.Name?.Value ?? "",
            Status = c.Status switch
            {
                ConversationStatus.Closed => "Encerrada",
                ConversationStatus.Open => "Aberta",
                ConversationStatus.JustCreated => "Nova",
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
                }
            }).ToList(),
        }).ToList();

        return new GetConversationsResponse
        {
            Conversations = conversations,
        };
    }
}