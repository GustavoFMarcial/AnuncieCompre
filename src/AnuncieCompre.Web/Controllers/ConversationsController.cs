using AnuncieCompre.Application.UseCases.Conversations;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetConversations([FromQuery] ConversationStatus? status, [FromServices] GetConversations service)
    {
        List<Conversation> conversations = await service.Handle(status);
        GetConversationsResponse response = conversations.ToGetConversationsResponse();
        
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetDetailedConversation([FromRoute] Guid id, [FromServices] GetDetailedConversation service)
    {
        Conversation? conversation = await service.Handle(id);

        if (conversation is null) return BadRequest("Conversation não encontrada");

        ConversationDTO response = conversation.ToConversationDTO();
        return Ok(response);
    }

    [HttpPost("{id:guid}/messages")]
    public async Task<ActionResult> SendMessage([FromRoute] Guid id, [FromBody] string text, [FromServices] SendMessage service)
    {
        Result result = await service.Handle(id, text);

        if (!result.IsSuccess) return BadRequest(result.Message);
        
        return Ok();
    }
}