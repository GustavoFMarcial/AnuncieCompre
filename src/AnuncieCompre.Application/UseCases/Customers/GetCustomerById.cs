using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases.Customers;

public class GetCustomerById(ICustomerRepository _customerRepository)
{
    private readonly ICustomerRepository customerRepository = _customerRepository;

    public async Task<Result<Customer>> Handle(Guid id)
    {
        Customer? customer = await customerRepository.GetByIdAsync(id);

        if (customer is null) return Result<Customer>.Failure("Customer não encontrado");

        return Result<Customer>.Success(customer, "Customer encontrado com sucesso");
    }
}