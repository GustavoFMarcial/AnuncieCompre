using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CustomerExtensions
{
    public static CustomerDTO ToCustomerDTO(this Customer customer)
    {
        return new CustomerDTO
        {
            Id = customer.Id,
            Phone = customer.Phone.Value,
            Name = customer.Name?.Value,
            Email = customer.Email?.Value,
        };
    }

    public static GetCustomersResponse ToListCustomerDTO(this List<Customer> customers)
    {
        List<CustomerDTO> c = customers.Select(c => new CustomerDTO
        {
            Id = c.Id,
            Phone = c.Phone.Value,
            Name = c.Name?.Value,
            Email = c.Email?.Value,
        }).ToList();


        return new GetCustomersResponse
        {
            Customers = c,
        };
    }
}