using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.DTO;

public record TwilioStatusCallbackInput
{
    public string MessageSid { get; set; } = default!;
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Body { get; set; } = default!;
    public MessageStatus MessageStatus { get; set; } = default!;
    public string ErrorCode { get; set; } = default!;
}