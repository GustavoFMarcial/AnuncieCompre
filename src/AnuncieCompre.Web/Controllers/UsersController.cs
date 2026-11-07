using AnuncieCompre.Application.UseCases.Users;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[EnableCors("Frontend")]
[Authorize(Roles = "Admin, Gestor")]
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

    [HttpPost("companies/{companyId:guid}")]
    public async Task<ActionResult> CreateUser([FromRoute] Guid companyId, [FromBody] CreateUserRequest request, [FromServices] CreateUser service)
    {
        CreateUserInput input = request.toCreateUserInput();

        Result result = await service.Handle(companyId, input);

        if (!result.IsSuccess) return BadRequest(new { success = false, errors = result.Messages });

        return Created();
    }
}