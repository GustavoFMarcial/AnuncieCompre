using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class User : BaseEntity
{
    public Phone Phone { get; private set; } = default!;
    public Name Name { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public List<ConversationAggregate.Conversation> Conversations = [];

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
}