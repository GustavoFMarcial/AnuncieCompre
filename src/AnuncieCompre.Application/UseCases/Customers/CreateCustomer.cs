using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Application.UseCases.Customers;

public class CreateCustomer(IUnitOfWork _unitOfWork)
{
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
        if (!nameResult.IsSuccess)
        {
            errors.Add(nameResult.Message);
        }
        if (!emailResult.IsSuccess)
        {
            errors.Add(emailResult.Message);
        }

        if (errors.Count > 0) return Result<Customer>.Failures(errors);

        Result<Customer> result = Customer.Create(phoneResult.Value);
        result.Value.SetName(nameResult.Value);
        result.Value.SetEmail(emailResult.Value);

        await unitOfWork.SaveChangesAsync();
        return Result<Customer>.Success(result.Value, result.Message);
    }
}