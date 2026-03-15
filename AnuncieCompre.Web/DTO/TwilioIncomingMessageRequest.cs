using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.DTO;

public class TwilioIncomingMessageRequest
{
    [FromForm(Name = "MessageSid")]
    public string MessageSid { get; set; } = string.Empty;

    [FromForm(Name = "From")]
    public string From { get; set; } = string.Empty;

    [FromForm(Name = "To")]
    public string To { get; set; } = string.Empty;

    [FromForm(Name = "Body")]
    public string Body { get; set; } = string.Empty;

    [FromForm(Name = "NumMedia")]
    public int NumMedia { get; set; }
}