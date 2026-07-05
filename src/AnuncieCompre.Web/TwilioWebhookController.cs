using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.AspNet.Core;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Twilio.TwiML.Messaging;
using AnuncieCompre.UseCase.ProcessMessageUseCase;
using AnuncieCompre.Domain.DTO;
using System.Collections.ObjectModel;

namespace AnuncieCompre.Web;

[ApiController]
[Route("webhooks/twilio")]
public class TwilioWebhookController(IProcessIncomingMessage _processMessageUseCase) : TwilioController
{
    private readonly IProcessIncomingMessage processMessageUseCase = _processMessageUseCase;

    [HttpPost("whatsapp")]
    public async Task<TwiMLResult> ReceiveMessage([FromForm] TwilioIncomingMessageRequest incomingMessage)
    {
        IncomingMessageRequest useCaseRequest = incomingMessage.ToUseCaseRequest();
        var response = new MessagingResponse();

        ReadOnlyCollection<string> result = await processMessageUseCase.ExecuteAsync(useCaseRequest);
        foreach(string r in result)
        {
            var message = new Message(r);
            response.Append(message);
        }

        return TwiML(response);
    }
}