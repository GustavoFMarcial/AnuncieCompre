using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    public Phone Phone { get; private set; } = default!;
    public UserType Type { get; private set; } = UserType.Create("1").Value;
    public Name? Name { get; private set; }
    public Email? Email { get; private set; }

    private User(){}

    private User(Phone phone)
    {
        Phone = phone;
    }

    public static User Create(Phone phone)
    {
        return new User(phone);
    }

    public User SetName(Name name)
    {
        Name = name;
        return this;
    }

    public User SetEmail(Email email)
    {
        Email = email;
        return this;
    }

    public User SetUserType(UserType userType)
    {
        Type = userType;
        return this;
    }
}