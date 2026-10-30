using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.Customers;

public class EditCustomer(ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork)
{
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> Handle(Guid id, EditCustomerInput input)
    {
        Customer? customer = await customerRepository.GetByIdAsync(id);

        if (customer is null) return Result.Failure("Customer não encontrado");

        List<string> errors = [];

        Result<Name> nameResult = Name.Create(input.Name);
        Result<Email> emailResult = Email.Create(input.Email);

        if (!nameResult.IsSuccess)
        {
            errors.Add(nameResult.Message);
        }

        if (!emailResult.IsSuccess)
        {
            errors.Add(emailResult.Message);
        }

        if (errors.Count > 0) return Result.Failures(errors);

        await unitOfWork.SaveChangesAsync();
        return Result.Success("Customer editado com sucesso");
    }
}