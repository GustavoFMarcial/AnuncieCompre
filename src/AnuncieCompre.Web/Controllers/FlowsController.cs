using AnuncieCompre.Application.UseCases;
using AnuncieCompre.Application.UseCases.Flows;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
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
    public async Task<ActionResult> GetFlows([FromServices] GetFlows getFlows)
    {
        List<ConversationFlow> flows = await getFlows.Handle();
        List<GetFlowsResponse> response = flows.ToGetFlowsResponse();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateFlow([FromBody] CreateFlowRequest input, [FromServices] CreateFlow createFlow)
    {
        CreateFlowInput request = input.ToCreateFlowRequest();
        Result<ConversationFlow> result = await createFlow.Handle(request);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateFlowResponse response = result.ToCreateFlowResponse();
        return CreatedAtAction(nameof(GetFlowById), response.Id, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetFlowById([FromRoute] Guid id, [FromServices] GetFlowById getFlowById)
    {
        ConversationFlow? flow = await getFlowById.Handle(id);

        if (flow is null) return BadRequest("Flow não encontrado");

        GetFlowByIdResponse response = flow.ToGetFlowByIdResponse();
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> EditFlow([FromRoute] Guid id, [FromBody] EditFlowRequest editFlowRequest, [FromServices] EditFlow editFlow)
    {
        EditFlowInput editFlowInput = editFlowRequest.ToEditFlowInout();
        Result result = await editFlow.Handle(id, editFlowInput);

        if (!result.IsSuccess) return BadRequest(result.Message);
    
        return Ok(result.Message);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteFlow([FromRoute] Guid id, [FromServices] DeleteFlow deleteFlow)
    {
        Result result = await deleteFlow.Handle(id);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return NoContent();
    }
}