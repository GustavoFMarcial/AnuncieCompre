using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace AnuncieCompre.Infra.Identity;

public class IdentityUserService(UserManager<ApplicationUser> _userManager, IRoleService _roleService, IUnitOfWork _unitOfWork) : IIdentityUserService
{
    private readonly UserManager<ApplicationUser> userManager = _userManager;
    private readonly IRoleService roleService = _roleService;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Result> CreateUserAsync(Guid companyId, Email email, Phone phone, Name name, CPF cpf, string login, string password, bool isAdmin = false)
    {
        List<string> errors = [];

        await using IApplicationTransaction transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
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

            if (isAdmin)
            {
                bool roleExist = await roleService.RoleExistAsync("Admin");

                if (!roleExist)
                {
                    await transaction.RollbackAsync();
                    return Result.Failure("Função admin não existe para criar conta admin");
                }

                Result roleResult = await AddToRoleAsync(user.Id, "Admin");

                if (!roleResult.IsSuccess)
                {
                    errors.AddRange(roleResult.Messages);

                    await transaction.RollbackAsync();
                    return Result.Failures(errors);
                }
            }

            await transaction.CommitTransactionAsync();
            return Result.Success("Usuário criado com sucesso");
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> AddToRoleAsync(Guid userId, string role)
    {
        List<string> errors = [];

        ApplicationUser? user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null) return Result.Failure("Usuário não encontrado");

        bool roleExist = await roleService.RoleExistAsync(role);

        if (!roleExist) return Result.Failure("Função não existe");

        IdentityResult roleResult = await userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            foreach (IdentityError e in roleResult.Errors)
            {
                errors.Add(e.Description);       
            }

            return Result.Failures(errors);
        }

        return Result.Success("Função adicionado ao usuário");
    }
}