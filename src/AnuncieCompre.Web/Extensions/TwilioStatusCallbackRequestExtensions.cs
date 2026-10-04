using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class TwilioStatusCallbackRequestExtensions
{
    public static TwilioStatusCallbackInput ToTwilioStatusCallbackInput(this TwilioStatusCallbackRequest request)
    {
        return new TwilioStatusCallbackInput
        {
            MessageSid = request.MessageSid,
            From = request.From,
            To = request.To,
            Body = request.Body,
            MessageStatus = request.MessageStatus.ToLower() switch
            {
                "queued" => MessageStatus.Queued,
                "sending" => MessageStatus.Sending,
                "sent" => MessageStatus.Sent,
                "delivered" => MessageStatus.Delivered,
                "read" => MessageStatus.Read,
                "failed" => MessageStatus.Failed,
                "undelivered" => MessageStatus.Undelivered,
                _ => MessageStatus.Failed,
            },
            ErrorCode = request.ErrorCode,
        };
    }
}