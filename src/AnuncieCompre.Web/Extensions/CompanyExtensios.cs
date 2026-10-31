using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CompanyExtensios
{
    public static CompanyDTO ToCompanyDTO(this Company company)
    {
        return new CompanyDTO
        {
            Id = company.Id,
            Name = company.Name.Value,
            Cnpj = company.Cnpj.Value,
            Email = company.Email.Value,
            Phone = company.Phone.Value,
        };
    }
}