using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using Twilio.Rest.Api.V2010.Account;

using Twilio.Types;

namespace AnuncieCompre.Infra.MessageSender;

public class TwilioMessageSender(MessageFailureHandler _messageFailureHandler) : IMessageSender
{
    private readonly MessageFailureHandler messageFailureHandler = _messageFailureHandler;

    public async Task SendMessageAsync(Message message)
    {
        var messageOptions = new CreateMessageOptions(new PhoneNumber(message.Conversation.Customer.Phone.Value))
        {
            From = new PhoneNumber("whatsapp:+14155238886"),
            Body = message.Text,
        };

        try
        {
            await MessageResource.CreateAsync(messageOptions);
        }
        catch(Exception e)
        {
            await messageFailureHandler.Handle(e, message);
        }
        
    }
}