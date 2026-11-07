using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.Users;

public class CreateUser(IIdentityUserService _identityUserService, ICompanyRepository _companyRepository, IApplicationUserRepository _applicationUserRepository)
{
    private readonly IIdentityUserService identityUserService = _identityUserService;
    private readonly ICompanyRepository companyRepository = _companyRepository;
    private readonly IApplicationUserRepository applicationUserRepository = _applicationUserRepository;

    public async Task<Result> Handle(Guid companyId, CreateUserInput input)
    {
        Company? company = await companyRepository.GetByIdAsync(companyId);

        if (company is null) return Result.Failure("Empresa não localizada");

        List<string> errors = [];

        Result<Email> emailResult = Email.Create(input.Email);
        Result<Phone> phoneResult = Phone.Create(input.Phone);
        Result<Name> nameResult = Name.Create(input.Name);
        Result<CPF> cpfResult = CPF.Create(input.Cpf);

        if (!emailResult.IsSuccess) errors.Add(emailResult.Message);
        if (!phoneResult.IsSuccess) errors.Add(phoneResult.Message);
        if (!nameResult.IsSuccess) errors.Add(nameResult.Message);
        if (!cpfResult.IsSuccess) errors.Add(cpfResult.Message);

        if (errors.Count > 0) return Result.Failures(errors);

        Result userResult = await identityUserService.CreateUserAsync(companyId, emailResult.Value, phoneResult.Value, nameResult.Value, cpfResult.Value, input.Login, input.Password);

        if (!userResult.IsSuccess) return Result.Failures(userResult.Messages);

        return Result.Success("Usuário criado com sucesso");
    }
}