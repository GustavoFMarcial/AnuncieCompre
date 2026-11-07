using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ApplicationUserRepository(AnuncieCompreContext _context, UserManager<ApplicationUser> _userManager) : IApplicationUserRepository
{
    private readonly AnuncieCompreContext context = _context;
    private readonly UserManager<ApplicationUser> userManager = _userManager;

    public async Task<ApplicationUserDTO?> GetByIdAsync(Guid id)
    {
        ApplicationUser? user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null) return null;

        IList<string> roles = await userManager.GetRolesAsync(user);

        ApplicationUserDTO userDTO = new ApplicationUserDTO
        {
            Id = user.Id,
            CompanyId = user.CompanyId,
            Company = user.Company.Name.Value,
            Email = user.Email,
            Phone = user.PhoneNumber,
            Name = user.Name.Value,
            Cpf = user.Cpf.Value,
            Roles = roles,
            CreatedAt = user.CreatedAt,
        };

        return userDTO;
    }

    public async Task<List<ApplicationUserDTO>> GetUsersByCompanyIdAsync(Guid companyId)
    {
        List<ApplicationUser> users = await context.Users
            .Where(u => u.CompanyId == companyId)
            .OrderBy(u => u.Name)
            .ToListAsync();

        List<ApplicationUserDTO> usersDTO = users
            .Select(u => new ApplicationUserDTO
            {
                Id = u.Id,
                CompanyId = u.CompanyId,
                Company = u.Company.Name.Value,
                Email = u.Email,
                Phone = u.PhoneNumber,
                Name = u.Name.Value,
                Cpf = u.Cpf.Value,
                CreatedAt = u.CreatedAt,
            })
            .ToList();
            
        for (int i = 0; i < users.Count; i++)
        {
            usersDTO[i].Roles = await userManager.GetRolesAsync(users[i]);
        }

        return usersDTO;
    }
}