using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    public Phone Phone { get; private set; } = default!;
    public Name? Name { get; private set; } = default!;
    public Email? Email { get; private set; } = default!;
    public UserType? Type { get; private set; } = default!;

    private User(){}

    private User(Phone phone, Name name, Email email, UserType userType)
    {
        Phone = phone;
        Name = name;
        Email = email;
        Type = userType;
    }

    public static User Create(Phone phone, Name name, Email email, UserType userType)
    {
        return new User(phone, name, email, userType);
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

    public User SetType(UserType userType)
    {
        Type = userType;
        return this;
    }
}