using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Services;

public class ValidateCompanyCategory
{
    public static Result<CompanyCategory> Validate(string message)
    {
        string[] values = Enum.GetValues<CompanyCategory>()
                        .Select(c => ((int)c).ToString())
                        .ToArray();

        foreach (string v in values)
        {
            if (v == message)
            {
                CompanyCategory companyCategory = (CompanyCategory)int.Parse(v);
                return Result<CompanyCategory>.Success(companyCategory, "");
            }
        }
        return Result<CompanyCategory>.Failure("Opção inválida, escolha novamente.");
    }
}