using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface IApplicationUserRepository
{
    public Task<ApplicationUserDTO?> GetByIdAsync(Guid id);
    public Task<List<ApplicationUserDTO>> GetUsersByCompanyIdAsync(Guid id);
}