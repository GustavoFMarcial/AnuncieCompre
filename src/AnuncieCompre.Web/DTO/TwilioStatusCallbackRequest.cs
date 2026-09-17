namespace AnuncieCompre.Web.DTO;

public record TwilioStatusCallbackRequest
{
    public string MessageSid { get; set; } = default!;
    public string From { get; set; } = default!;
    public string To { get; set; } = default!;
    public string Body { get; set; } = default!;
    public string MessageStatus { get; set; } = default!;
    public string ErrorCode { get; set; } = default!;
}