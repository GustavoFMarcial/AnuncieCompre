using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class Customer : BaseEntity
{
    public Phone Phone { get; private set; } = default!;
    public Name? Name { get; private set; }
    public Email? Email { get; private set; }
    public List<ConversationAggregate.Conversation> Conversations = [];

    private Customer(){}

    private Customer(Phone phone)
    {
        Phone = phone;
    }

    public static Customer Create(Phone phone)
    {
        return new Customer(phone);
    }

    public Customer SetName(Name name)
    {
        Name = name;
        return this;
    }

    public Customer SetEmail(Email email)
    {
        Email = email;
        return this;
    }
}