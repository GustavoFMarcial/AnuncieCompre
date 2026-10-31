using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;
using Microsoft.AspNetCore.Identity;

namespace AnuncieCompre.Infra.Identity;

public class ApplicationUserService(UserManager<ApplicationUser> _userManager) : IUserService
{
    private readonly UserManager<ApplicationUser> userManager = _userManager;

    public async Task<Result> CreateUserAsync(Guid companyId, Email email, Phone phone, Name name, CPF cpf, string login, string password)
    {
        List<string> errors = [];

        ApplicationUser user = new ApplicationUser
        {
            CompanyId = companyId,
            UserName = login,
            Name = name,
            Email = email.Value,
            Cpf = cpf,
            PhoneNumber = phone.Value,
        };

        IdentityResult result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            foreach (IdentityError e in result.Errors)
            {
                errors.Add(e.Description);       
            }

            return Result.Failures(errors);
        }

        return Result.Success("User criado com sucesso");
    }
}