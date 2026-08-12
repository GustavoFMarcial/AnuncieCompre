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
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetFlowById([FromRoute] Guid id, [FromServices] GetFlowById getFlowById)
    {
        ConversationFlow? flow = await getFlowById.Handle(id);

        if (flow is null) return BadRequest("Flow não encontrado");
        
        GetFlowByIdResponse response = flow.ToGetFlowByIdResponse();
        return Ok(response);
    }
    public async Task<ActionResult> CreateFlow([FromBody] CreateFlowRequest input, [FromServices] CreateFlow createFlow)
    {
        CreateFlowInput request = input.ToCreateFlowRequest();
        Result<ConversationFlow> result = await createFlow.Handle(request);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CreateFlowResponse response = result.ToCreateFlowResponse();

        return CreatedAtAction(nameof(GetFlowById), response.Id, response);
    }
}