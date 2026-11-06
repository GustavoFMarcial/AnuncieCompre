using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class ApplicationUserRepository(AnuncieCompreContext _context) : IApplicationUserRepository
{
    private readonly AnuncieCompreContext context = _context;

    public async Task<ApplicationUserDTO?> GetByIdAsync(Guid id)
    {
        ApplicationUserDTO? user = await context.Users
            .Where(u => u.Id == id)
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
            .FirstOrDefaultAsync();

        return user;
    }

    public async Task<List<ApplicationUserDTO>> GetUsersByCompanyIdAsync(Guid companyId)
    {
        return await context.Users
            .Where(u => u.CompanyId == companyId)
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
            .ToListAsync();
    }
}