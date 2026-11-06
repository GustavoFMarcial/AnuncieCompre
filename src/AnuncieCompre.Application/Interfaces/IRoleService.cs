using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.Interfaces;

public interface IRoleService
{
    public Task<bool> RoleExistAsync(string role);
}