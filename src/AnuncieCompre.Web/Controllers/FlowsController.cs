using AnuncieCompre.Application.UseCases.Flows;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlowsController : ControllerBase
{
    [HttpGet("{id}")]
    [HttpPost]
    public ActionResult GetFlowById([FromQuery] int id)
    {
        return Ok();
    }
    public async Task<ActionResult> CreateFlow([FromBody] CreateFlowInput input, [FromServices] CreateFlow createFlow)
    {
        CreateFlowRequest request = input.ToCreateFlowRequest();
        Flow flow = await createFlow.Handle(request);
        return CreatedAtAction(nameof(GetFlowById), flow.Id, flow);
    }
}