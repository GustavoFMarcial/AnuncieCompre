using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Aggregates.CustomerAggregate;

public class Customer : BaseEntity
{
    // public Guid CompanyId { get; private set; }
    // public Company Company { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;
    public Name? Name { get; private set; }
    public Email? Email { get; private set; }
    public List<ConversationAggregate.Conversation> Conversations = [];

    private Customer(){}

    private Customer(Phone phone)
    {
        Phone = phone;
    }

    public static Result<Customer> Create(Phone phone)
    {
        Customer customer = new Customer(phone);
        return Result<Customer>.Success(customer, "Customer criado com sucesso");
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