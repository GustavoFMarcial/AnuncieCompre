using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Application.UseCases.Customers;

public class CreateCustomer(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork)
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result<Customer>> Handle(CreateCustomerInput input)
    {
        List<string> errors = [];
        Result<Phone> phoneResult = Phone.Create(input.Phone);
        Result<Name> nameResult = Name.Create(input.Name);
        Result<Email> emailResult = Email.Create(input.Email);

        if (!phoneResult.IsSuccess)
        {
            errors.Add(phoneResult.Message);
        }

        Customer? customer = await customerRepository.GetCustomerByPhoneAsync(input.Phone);

        if (customer is not null) return Result<Customer>.Failure("Customer já cadastrado");

        if (!nameResult.IsSuccess)
        {
            errors.Add(nameResult.Message);
        }
        if (!emailResult.IsSuccess)
        {
            errors.Add(emailResult.Message);
        }

        if (errors.Count > 0) return Result<Customer>.Failures(errors);

        Result<Customer> customerResult = Customer.Create(phoneResult.Value);
        customerResult.Value.SetName(nameResult.Value);
        customerResult.Value.SetEmail(emailResult.Value);

        customerRepository.Add(customerResult.Value);

        await unitOfWork.SaveChangesAsync();
        return Result<Customer>.Success(customerResult.Value, customerResult.Message);
    }
}