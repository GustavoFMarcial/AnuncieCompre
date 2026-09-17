using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Application.Services;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;
using Twilio.Rest.Api.V2010.Account;

using Twilio.Types;

namespace AnuncieCompre.Infra.MessageSender;

public class TwilioMessageSender(MessageFailureHandler _messageFailureHandler) : IMessageSender
{
    private readonly MessageFailureHandler messageFailureHandler = _messageFailureHandler;

    public async Task<Result<MessageProviderReference>> SendMessageAsync(Message message)
    {
        var messageOptions = new CreateMessageOptions(new PhoneNumber(message.Conversation.Customer.Phone.Value))
        {
            From = new PhoneNumber("whatsapp:+14155238886"),
            Body = message.Text,
            StatusCallback = new Uri("https://unwavering-kian-inadmissibly.ngrok-free.dev/webhooks/twilio/whatsapp/message-status"),
        };

        try
        {
            var messageResource = await MessageResource.CreateAsync(messageOptions);
            Result<MessageProviderReference> messageReference = MessageProviderReference.Create(message, MessageProvider.Twilio, messageResource.Sid);
            return Result<MessageProviderReference>.Success(messageReference.Value, "Mensagem enviada");
        }
        catch(Exception e)
        {
            await messageFailureHandler.Handle(e, message);
            return Result<MessageProviderReference>.Failure("Mensagem não enviada");
        }
    }
}