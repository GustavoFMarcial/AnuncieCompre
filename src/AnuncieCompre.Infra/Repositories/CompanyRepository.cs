using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories;

public class CompanyRepository(AnuncieCompreContext _context) : BaseRepository<Company>(_context), ICompanyRepository
{
    public async Task<Company?> GetCompanyByCnpjAsync(string cnpj)
    {
        return await context.Set<Company>().FirstOrDefaultAsync(c => c.Cnpj.Value == cnpj);
    }
}