using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.Companies;

public class CreateCompany(ICompanyRepository _companyRepository, IUserService _userService, IUnitOfWork _unitOfWork)
{
    private readonly ICompanyRepository companyRepository = _companyRepository;
    private readonly IUserService userService = _userService;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result<Company>> Handle(CreateCompanyInput companyInput, CreateUserInput userInput)
    {
        Company? company = await companyRepository.GetCompanyByCnpjAsync(companyInput.Cnpj);

        if (company is not null) return Result<Company>.Failure("Já existe empresa cadastrada com esse CNPJ");

        List<string> errors = [];

        Result<Name> companyNameResult = Name.Create(companyInput.CompanyName);
        Result<CNPJ> cnpjResult = CNPJ.Create(companyInput.Cnpj);
        Result<Email> emailResult = Email.Create(companyInput.Email);
        Result<Phone> companyPhoneResult = Phone.Create(companyInput.Phone);

        Result<Name> userNameResult = Name.Create(userInput.Name);
        Result<CPF> cpfResult = CPF.Create(userInput.Cpf);

        if (!companyNameResult.IsSuccess) errors.Add(companyNameResult.Message);
        if (!cnpjResult.IsSuccess) errors.Add(cnpjResult.Message);
        if (!emailResult.IsSuccess) errors.Add(emailResult.Message);
        if (!companyPhoneResult.IsSuccess) errors.Add(companyPhoneResult.Message);
        if (!userNameResult.IsSuccess) errors.Add(userNameResult.Message);
        if (!cpfResult.IsSuccess) errors.Add(cpfResult.Message);

        if (errors.Count > 0) return Result<Company>.Failures(errors);

        await using IApplicationTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            Result<Company> companyResult = Company.Create(companyNameResult.Value, cnpjResult.Value, emailResult.Value, companyPhoneResult.Value);

            if (!companyResult.IsSuccess) return Result<Company>.Failure(companyResult.Message);

            companyRepository.Add(companyResult.Value);
            await unitOfWork.SaveChangesAsync();

            Result userResult = await userService.CreateUserAsync(companyResult.Value.Id, emailResult.Value, companyPhoneResult.Value, userNameResult.Value, cpfResult.Value, userInput.Login, userInput.Password);

            if (!userResult.IsSuccess) return Result<Company>.Failures(userResult.Messages);       

            await transaction.CommitTransactionAsync();
            return Result<Company>.Success(companyResult.Value, "Company criado com sucesso");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result<Company>.Failure(ex.Message);
        }
    }
}