using AnuncieCompre.UseCase.Interfaces;
using Twilio.Rest.Api.V2010.Account;

using Twilio.Types;

namespace AnuncieCompre.Infra.MessageSender;

public class TwilioMessageSender : IMessageSender
{
    public async Task SendMessageAsync(string to, string message)
    {
        await MessageResource.CreateAsync(
            from: new PhoneNumber($"whatsapp:+14155238886"),
            to: new PhoneNumber(to),
            body: message
        );
    }
}