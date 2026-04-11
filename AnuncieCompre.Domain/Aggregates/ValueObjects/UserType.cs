using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Enums;

namespace AnuncieCompre.Domain.Aggregates.ValueObjects;

public class UserType : ValueObject
{
    public Enums.UserType Value { get; private set; }

    private UserType(){}
    private UserType(Enums.UserType userType)
    {
        Value = userType;
    }

    public static Result<UserType> Create(string userType)
    {
        string[] userTypes = UserTypeExtensions.ToStringArray();

        foreach (string u in userTypes)
        {
            if (u == userType) return Result<UserType>.Success(new UserType((Enums.UserType)int.Parse(userType)), "UserType validado com sucesso");
        }
        
        return Result<UserType>.Failure("Opção inválida, escolha novamente.");
    }
}