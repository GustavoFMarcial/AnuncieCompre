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
    public async Task<ActionResult> GetConversationFlows([FromServices] GetConversationFlows service)
    {
        List<ConversationFlow> flows = await service.Handle();
        List<GetConversationFlowsResponse> response = flows.ToGetConversationFlowsResponse();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateConversationFlow([FromBody] CreateConversationFlowRequest input, [FromServices] CreateConversationFlow service)
    {
        CreateConversationFlowInput request = input.ToCreateConversationFlowRequest();
        Result<ConversationFlow> result = await service.Handle(request);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateConversationFlowResponse response = result.ToCreateConversationFlowResponse();
        return CreatedAtAction(nameof(GetConversationFlowById), response.Id, response);
    }

    [HttpGet("{flowId:guid}")]
    public async Task<ActionResult> GetConversationFlowById([FromRoute] Guid flowId, [FromServices] GetConversationFlowById service)
    {
        ConversationFlow? flow = await service.Handle(flowId);

        if (flow is null) return BadRequest("Flow não encontrado");

        GetConversationFlowByIdResponse response = flow.ToGetConversationFlowByIdResponse();
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

    [HttpDelete("{flowId:guid}")]
    public async Task<ActionResult> DeleteConversationFlow([FromRoute] Guid flowId, [FromServices] DeleteConversationFlow service)
    {
        Result result = await service.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return NoContent();
    }

    [HttpPost("{flowId:guid}")]
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
    public async Task<ActionResult> EditConversationNodeTransition([FromRoute] Guid flowId, [FromRoute] Guid nodeId, [FromBody] List<EditConversationNodeTransitionRequest> request, [FromServices] EditConversationNodeTransitions service)
    {
        List<EditConversationNodeTransitionInput> input = request.ToEditConversationNodeTransitionInput();

        Result result = await service.Handle(nodeId, input);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return Ok();
    }
}