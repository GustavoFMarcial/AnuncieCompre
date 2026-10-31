using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Application.UseCases.Companies;

public class GetCompanyById(ICompanyRepository _companyRepository)
{
    private readonly ICompanyRepository companyRepository = _companyRepository;

    public async Task<Result<Company>> Handle(Guid companyId)
    {
        Company? company = await companyRepository.GetByIdAsync(companyId);

        if (company is null) return Result<Company>.Failure("Company não encontrada");

        return Result<Company>.Success(company, "Company encontrada com sucesso");
    }
}