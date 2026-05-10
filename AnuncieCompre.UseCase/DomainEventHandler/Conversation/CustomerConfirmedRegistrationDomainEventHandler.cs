using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerConfirmedRegistrationDomainEventHandler(IDatabase _db, ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<CustomerConfirmedRegistrationDomainEvent>
{
    private readonly IDatabase db = _db;
    private readonly ICustomerRepository customerRepository = _customerRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(CustomerConfirmedRegistrationDomainEvent domainEvent)
    {
        string key = $"user:{domainEvent.User.Phone.Value}";
        var entries = await db.HashGetAllAsync(key);

        var data = entries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        );

        var name = JsonSerializer.Deserialize<Name>(data["name"]);
        var email = JsonSerializer.Deserialize<Email>(data["email"]);
        var type = JsonSerializer.Deserialize<UserType>(data["type"]);
        var cpf = JsonSerializer.Deserialize<CPF>(data["cpf"]);

        if (name is null || email is null || type is null || cpf is null) return;

        domainEvent.User
            .SetName(name)
            .SetEmail(email)
            .SetUserType(type);

        Customer customer = Customer.Create(domainEvent.User, cpf);
        customerRepository.Add(customer);

        await db.KeyDeleteAsync(key);
        await unitOfWork.SaveChangesAsync();
    }
}