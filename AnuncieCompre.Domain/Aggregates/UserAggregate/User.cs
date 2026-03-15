using System.Reflection.Metadata;
using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    public VOPhone Phone { get; private set; } = default!;
    public VOName? Name { get; private set; } = default!;
    public VOEmail? Email { get; private set; } = default!;
    public UserType? Type { get; private set; } = UserType.Unknown;

    private User(){}

    private User(VOPhone phone)
    {
        Phone = phone;
    }

    public static User Create(VOPhone phone)
    {
        return new User(phone);
    }

    public User SetName(VOName name)
    {
        Name = name;
        return this;
    }

    public User SetEmail(VOEmail email)
    {
        Email = email;
        return this;
    }

    public User SetType(UserType userType)
    {
        Type = userType;
        return this;
    }
}