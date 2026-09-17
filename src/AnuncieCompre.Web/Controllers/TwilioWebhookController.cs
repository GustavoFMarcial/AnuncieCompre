using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.AspNet.Core;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Twilio.TwiML.Messaging;
using AnuncieCompre.Application.UseCases.ProcessMessageUseCase;
using AnuncieCompre.Domain.DTO;
using System.Collections.ObjectModel;
using AnuncieCompre.Web.Filters;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[Route("webhooks/twilio")]
public class TwilioWebhookController() : TwilioController
{
    [HttpPost("whatsapp")]
    [ValidateTwilioRequest]
    public async Task<TwiMLResult> ReceiveMessage([FromForm] TwilioIncomingMessageRequest incomingMessage, [FromServices] IProcessIncomingMessage service)
    {
        IncomingMessageRequest useCaseRequest = incomingMessage.ToUseCaseRequest();
        var response = new MessagingResponse();

        ReadOnlyCollection<string> result = await service.ExecuteAsync(useCaseRequest);
        foreach(string r in result)
        {
            var message = new Message(r);
            response.Append(message);
        }

        return TwiML(response);
    }

    [HttpPost("whatsapp/message-status")]
    [ValidateTwilioRequest]
    public async Task<ActionResult> MessageStatus([FromForm] TwilioStatusCallbackRequest request, [FromServices] ProcessMessageStatus service)
    {
        TwilioStatusCallbackInput input = request.ToTwilioStatusCallbackInput();
        await service.Handle(input);
        return Ok();
    }
}