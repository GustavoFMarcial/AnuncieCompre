using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.DTO;

namespace AnuncieCompre.Application.UseCases.Users;

public class GetUsers(IApplicationUserRepository _applicationUserRepository)
{
    private readonly IApplicationUserRepository applicationUserRepository = _applicationUserRepository;

    public async Task<List<ApplicationUserDTO>> Handle(Guid companyId)
    {
        return await applicationUserRepository.GetUsersByCompanyIdAsync(companyId);
    }
}