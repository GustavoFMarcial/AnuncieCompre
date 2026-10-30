using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;

namespace AnuncieCompre.Application.UseCases.Customers;

public class GetCustomers(ICustomerRepository _customerRepository)
{
    private readonly ICustomerRepository customerRepository = _customerRepository;

    public async Task<List<Customer>> Handle()
    {
        return await customerRepository.ToListAsync();
    }
}