using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CreateCompanyRequestExtensions
{
    public static CreateCompanyInput ToCreateCompanyInput(this CreateCompanyRequest request)
    {
        return new CreateCompanyInput
        {
            CompanyName = request.CompanyName,
            Cnpj = request.Cnpj,
            Email = request.Email,
            Phone = request.Phone,
        };
    }

    public static CreateUserInput ToCreateUserInput(this CreateCompanyRequest request)
    {
        return new CreateUserInput
        {
            Name = $"admin-{request.CompanyName}",
            Email = request.Email,
            Phone = request.Phone,
            Cpf = request.Cpf,
            Login = request.Login,
            Password = request.Password,
        };
    }
}