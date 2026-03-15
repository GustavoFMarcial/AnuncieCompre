using AnuncieCompre.Domain.Aggregates.ValueObjects;

namespace AnuncieCompre.Domain.Aggregates.UserAggregate;

public class Customer : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; } = default!;
    public VOCPF CPF { get; private set; } = default!;

    private Customer(){}

    private Customer(User user, VOCPF cpf)
    {
        UserId = user.Id;
        User = user;
        CPF = cpf;
    }

    public static Customer Create(User user, VOCPF cpf)
    {
        return new Customer(user, cpf);
    }
}