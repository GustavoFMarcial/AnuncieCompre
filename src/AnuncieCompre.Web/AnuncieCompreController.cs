using AnuncieCompre.Web.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web;

[ApiController]
[Route("api/[controller]")]
public class AnuncieCompreController : ControllerBase
{
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