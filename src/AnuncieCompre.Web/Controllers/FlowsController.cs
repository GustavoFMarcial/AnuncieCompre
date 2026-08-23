using AnuncieCompre.Application.UseCases;
using AnuncieCompre.Application.UseCases.Flows;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlowsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetConversationFlows([FromServices] GetConversationFlows service)
    {
        List<ConversationFlow> flows = await service.Handle();
        List<GetConversationFlowsResponse> response = flows.ToGetConversationFlowsResponse();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateConversationFlow([FromBody] CreateConversationFlowRequest request, [FromServices] CreateConversationFlow service)
    {
        CreateConversationFlowInput input = request.ToCreateConversationFlowRequest();
        Result<ConversationFlow> result = await service.Handle(input);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateConversationFlowResponse response = result.ToCreateConversationFlowResponse();
        return CreatedAtAction(nameof(GetConversationFlowById), new { flowId = response.Id }, response);
    }

    [HttpGet("{flowId:guid}")]
    public async Task<ActionResult> GetConversationFlowById([FromRoute] Guid flowId, [FromServices] GetConversationFlowById service)
    {
        Result<ConversationFlow> result = await service.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        GetConversationFlowByIdResponse response = result.Value.ToGetConversationFlowByIdResponse();
        return Ok(response);
    }

    [HttpPut("{flowId:guid}")]
    public async Task<ActionResult> EditConversationFlow([FromRoute] Guid flowId, [FromBody] EditConversationFlowRequest request, [FromServices] EditConversationFlow service)
    {
        EditConversationFlowInput editFlowInput = request.ToConversationEditFlowInout();
        Result result = await service.Handle(flowId, editFlowInput);

        if (!result.IsSuccess) return BadRequest(result.Message);
    
        return Ok(result.Message);
    }

    [HttpPatch("{flowId:guid}/status")]
    public async Task<ActionResult> EditConversationFlowStatus([FromRoute] Guid flowId, [FromBody] EditConversationFlowStatusRequest request, [FromServices] EditConversationFlowStatus service)
    {
        EditConversationFlowStatusInput input = request.ToEditConversationFlowStatusInput();
        Result result = await service.Handle(flowId, input);

        if (!result.IsSuccess) return BadRequest(result.Message.Split(",").ToList());

        return Ok(result.Message);
    }

    [HttpDelete("{flowId:guid}")]
    public async Task<ActionResult> DeleteConversationFlow([FromRoute] Guid flowId, [FromServices] DeleteConversationFlow service)
    {
        Result result = await service.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return NoContent();
    }

    [HttpPost("{flowId:guid}/nodes")]
    public async Task<ActionResult> CreateConversationNode([FromRoute] Guid flowId, [FromServices] CreateConversationNode service)
    {
        Result<ConversationNode> result = await service.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateConversationNodeResponse response = result.Value.ToCreateConversationNodeResponse();
        return Created(response.Id, response);
    }

    [HttpPut("{flowId:guid}/nodes/{nodeId:guid}")]
    public async Task<ActionResult> EditConversationNode([FromRoute] Guid flowId, [FromRoute] Guid nodeId, [FromBody] EditConversationNodeRequest request, [FromServices] EditConversationNode service)
    {
        EditConversationNodeInput input = request.ToEditConversationNodeInput();
        Result<ConversationNode> result = await service.Handle(nodeId, input);

        if (!result.IsSuccess) return BadRequest(result.Message);

        EditConversationNodeResponse response = result.ToEditConversationNodeResponse();
        return Ok(response);
    }

    [HttpPatch("{flowId:guid}/nodes/{nodeId:guid}/transitions")]
    public async Task<ActionResult> EditConversationNodeTransition([FromRoute] Guid flowId, [FromRoute] Guid nodeId, [FromBody] EditConversationNodeTransitionRequest request, [FromServices] EditConversationNodeTransitions service)
    {
        EditConversationNodeTransitionInput input = request.ToEditConversationNodeTransitionInput();

        Result result = await service.Handle(nodeId, input);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return Ok();
    }

    [HttpDelete("{flowId:guid}/nodes/{nodeId:guid}")]
    public async Task<ActionResult> DeleteConversationNode([FromRoute] Guid flowId, [FromRoute] Guid nodeId, [FromServices] DeleteConversationNode service)
    {
        Result result = await service.Handle(nodeId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return Ok();
    }
}