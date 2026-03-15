using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class TwilioRequestExtensions
{
    public static IncomingMessageRequest ToUseCaseRequest(this TwilioIncomingMessageRequest incomingMessageRequest)
    {
        return new IncomingMessageRequest
        {
            MessageId = incomingMessageRequest.MessageSid,
            SenderPhone = incomingMessageRequest.From,
            RecipientPhone = incomingMessageRequest.To,
            Content = incomingMessageRequest.Body,
            HasAttachments = incomingMessageRequest.NumMedia > 0,
        };
    }
}