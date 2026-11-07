using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Web.DTO;

namespace AnuncieCompre.Web.Extensions;

public static class CreateUserRequestExtensions
{
    public static CreateUserInput toCreateUserInput(this CreateUserRequest request)
    {
        return new CreateUserInput
        {
            Name = request.Name,
            Cpf = request.Cpf,
            Phone = request.Phone,
            Email = request.Email,
            Login = request.Login,
            Password = request.Login,
        };
    }
}