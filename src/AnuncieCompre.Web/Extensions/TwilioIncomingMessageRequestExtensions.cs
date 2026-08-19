using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class TwilioIncomingMessageRequestExtensions
{
    public static IncomingMessageRequest ToUseCaseRequest(this TwilioIncomingMessageRequest request)
    {
        return new IncomingMessageRequest
        {
            MessageId = request.MessageSid,
            SenderPhone = request.From,
            RecipientPhone = request.To,
            Content = request.Body,
            HasAttachments = request.NumMedia > 0,
        };
    }
}