using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace AnuncieCompre.Infra.Identity;

public class RoleService(RoleManager<IdentityRole<Guid>> _roleManager) : IRoleService
{
    private readonly RoleManager<IdentityRole<Guid>> roleManager = _roleManager;

    public async Task<bool> RoleExistAsync(string role)
    {
        return await roleManager.RoleExistsAsync(role);
    }
}