using AnuncieCompre.Application.Interfaces;
using Twilio.Rest.Api.V2010.Account;

using Twilio.Types;

namespace AnuncieCompre.Infra.MessageSender;

public class TwilioMessageSender : IMessageSender
{
    public async Task SendMessageAsync(string to, string message)
    {
        var messageOptions = new CreateMessageOptions(new PhoneNumber(to))
        {
            From = new PhoneNumber("whatsapp:+14155238886"),
            Body = message
        };

        try
        {
            await MessageResource.CreateAsync(messageOptions);
        }
        catch(Exception e)
        {
            //TODO - incluir tratamento de exception
            Console.WriteLine(e);
        }
        
    }
}