using AnuncieCompre.Application.UseCases.Users;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[EnableCors("Frontend")]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{userId:guid}/companies/{comapanyId:guid}")]
    public async Task<ActionResult> GetUserById([FromRoute] Guid userId, [FromRoute] Guid comapanyId, [FromServices] GetUserById service)
    {
        Result<ApplicationUserDTO> result = await service.Handle(userId, comapanyId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        return Ok(result.Value);
    }

    [HttpGet("companies/{companyId:guid}")]
    public async Task<ActionResult> GetUsers([FromRoute] Guid companyId, [FromServices] GetUsers service)
    {
        List<ApplicationUserDTO> users = await service.Handle(companyId);

        return Ok(users);
    }
}