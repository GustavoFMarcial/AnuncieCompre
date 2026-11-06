using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.Users;

public class GetUserById(IApplicationUserRepository _applicationUserRepository)
{
    private readonly IApplicationUserRepository applicationUserRepository = _applicationUserRepository;

    public async Task<Result<ApplicationUserDTO>> Handle(Guid userId, Guid companyId)
    {
        ApplicationUserDTO? user =  await applicationUserRepository.GetByIdAsync(userId);

        if (user is null) return Result<ApplicationUserDTO>.Failure("Usuário não encontrado");

        if (user.CompanyId != companyId) return Result<ApplicationUserDTO>.Failure("Usuário não pertence a empresa");

        return Result<ApplicationUserDTO>.Success(user, "Usuário encontrado");
    }
}