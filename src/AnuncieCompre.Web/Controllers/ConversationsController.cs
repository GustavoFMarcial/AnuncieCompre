using AnuncieCompre.Application.UseCases.Conversations;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
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

    [HttpGet]
    public async Task<ActionResult> GetDetailedConversation([FromRoute] Guid id, [FromServices] GetDetailedConversation service)
    {
        Conversation? conversation = await service.Handle(id);

        if (conversation is null) return BadRequest("Conversation não encontrada");

        ConversationDTO response = conversation.ToConversationDTO();
        return Ok(response);
    }
}