using AnuncieCompre.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlowsController : ControllerBase
{
    [HttpGet("ok")]
    public ActionResult Get()
    {
        return Ok();
    }

    public ActionResult CreateNode([FromForm] CreateNodeData nodeData)
    {
        Console.WriteLine(nodeData.Text);
        Console.WriteLine(nodeData.Validation);
        
        return Ok();
    }
}