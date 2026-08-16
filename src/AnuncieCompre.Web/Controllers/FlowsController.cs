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
    public async Task<ActionResult> GetFlows([FromServices] GetConversationFlows getConversationFlows)
    {
        List<ConversationFlow> flows = await getConversationFlows.Handle();
        List<GetConversationFlowsResponse> response = flows.ToGetConversationFlowsResponse();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateFlow([FromBody] CreateConversationFlowRequest input, [FromServices] CreateConversationFlow createConversationFlow)
    {
        CreateConversationFlowInput request = input.ToCreateConversationFlowRequest();
        Result<ConversationFlow> result = await createConversationFlow.Handle(request);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateConversationFlowResponse response = result.ToCreateConversationFlowResponse();
        return CreatedAtAction(nameof(GetFlowById), response.Id, response);
    }

    [HttpGet("{flowId:guid}")]
    public async Task<ActionResult> GetFlowById([FromRoute] Guid flowId, [FromServices] GetConversationFlowById getConversationFlowById)
    {
        ConversationFlow? flow = await getConversationFlowById.Handle(flowId);

        if (flow is null) return BadRequest("Flow não encontrado");

        GetConversationFlowByIdResponse response = flow.ToGetConversationFlowByIdResponse();
        return Ok(response);
    }

    [HttpPut("{flowId:guid}")]
    public async Task<ActionResult> EditFlow([FromRoute] Guid flowId, [FromBody] EditConversationFlowRequest editFlowRequest, [FromServices] EditConversationFlow editConversationFlow)
    {
        EditConversationFlowInput editFlowInput = editFlowRequest.ToConversationEditFlowInout();
        Result result = await editConversationFlow.Handle(flowId, editFlowInput);

        if (!result.IsSuccess) return BadRequest(result.Message);
    
        return Ok(result.Message);
    }

    [HttpDelete("{flowId:guid}")]
    public async Task<ActionResult> DeleteFlow([FromRoute] Guid flowId, [FromServices] DeleteConversationFlow deleteConversationFlow)
    {
        Result result = await deleteConversationFlow.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return NoContent();
    }

    [HttpPost("{flowId:guid}")]
    public async Task<ActionResult> CreateNode([FromRoute] Guid flowId, [FromServices] CreateConversationNode createConversationNode)
    {
        Result<ConversationNode> result = await createConversationNode.Handle(flowId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateConversationNodeResponse response = result.Value.ToCreateConversationNodeResponse();
        return Created(response.Id, response);
    }
}