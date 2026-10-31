using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Application.Interfaces;

public interface ICompanyRepository : IBaseRepository<Company>
{
    public Task<Company?> GetCompanyByCnpjAsync(string cnpj);
}