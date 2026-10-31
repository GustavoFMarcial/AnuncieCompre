using AnuncieCompre.Application.UseCases.Companies;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    [HttpGet("{companyId:guid}")]
    public async Task<ActionResult> GetCompanyById([FromRoute] Guid companyId, [FromServices] GetCompanyById service)
    {
        Result<Company> result = await service.Handle(companyId);

        if (!result.IsSuccess) return BadRequest(result.Message);
        CompanyDTO response = result.Value.ToCompanyDTO();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCompany([FromBody] CreateCompanyRequest request, [FromServices] CreateCompany service)
    {
        CreateCompanyInput companyInput = request.ToCreateCompanyInput();
        CreateUserInput userInput = request.ToCreateUserInput();

        Result<Company> result = await service.Handle(companyInput, userInput);

        if (!result.IsSuccess) return BadRequest(new { success = false, errors = result.Messages });

        CompanyDTO response = result.Value.ToCompanyDTO();

        return CreatedAtAction(nameof(GetCompanyById), new { companyId = response.Id }, response);
    }
}