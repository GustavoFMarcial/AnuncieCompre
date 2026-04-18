using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Repositories.VendorRepo;

public class VendorRepository(AnuncieCompreContext _context) : BaseRepository<Vendor>(_context), IVendorRepository
{
    public async Task<List<Vendor>> GetVendorsByCategoryAsync(CompanyCategory category)
    {
        return await context.Set<Vendor>().Where(v => v.Category.Value == category).ToListAsync();
    }
}