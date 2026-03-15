using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Enums;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.UseCase.Interfaces;

public interface IVendorRepository : IBaseRepository<Vendor>
{
    public Task<List<Vendor>> GetVendorsByCategoryAsync(CompanyCategory category);
}