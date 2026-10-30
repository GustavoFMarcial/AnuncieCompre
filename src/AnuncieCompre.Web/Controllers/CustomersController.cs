using AnuncieCompre.Application.UseCases.Customers;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;
using AnuncieCompre.Web.Extensions;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AnuncieCompre.Web.Controllers;

[ApiController]
[EnableCors("Frontend")]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetCustomers([FromServices] GetCustomers service)
    {
        List<Customer> customers = await service.Handle();
        GetCustomersResponse response = customers.ToListCustomerDTO();

        return Ok(response);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult> GetCustomerById([FromRoute] Guid customerId, [FromServices] GetCustomerById service)
    {
        Result<Customer> result = await service.Handle(customerId);

        if (!result.IsSuccess) return BadRequest(result.Message);

        CustomerDTO response = result.Value.ToCustomerDTO();
        return Ok(response);
    }

    [HttpPatch("{customerId:guid}")]
    public async Task<ActionResult> EditCustomer([FromRoute] Guid customerId, [FromBody] EditCustomerRequest request, [FromServices] EditCustomer service)
    {
        EditCustomerInput input = request.ToEditCustomerInput();
        Result result = await service.Handle(customerId, input);

        if (!result.IsSuccess) return BadRequest(new { success = false, errors = result.Messages });

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, [FromServices] CreateCustomer service)
    {
        CreateCustomerInput input = request.ToCreateCustomerInput();
        Result<Customer> result = await service.Handle(input);

        if (!result.IsSuccess) return BadRequest(result.Messages);
        CustomerDTO response = result.Value.ToCustomerDTO();

        return CreatedAtAction(nameof(GetCustomerById), new { customerId = response.Id }, response);
    }
}