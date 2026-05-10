using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorConfirmedRegistrationDomainEventHandler(IDatabase _db, IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorConfirmedRegistrationDomainEvent>
{
    private readonly IDatabase db = _db;
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorConfirmedRegistrationDomainEvent domainEvent)
    {
        string key = $"user{domainEvent.User.Phone.Value}";
        var entries = await db.HashGetAllAsync(key);

        var data = entries.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        );

        var name = JsonSerializer.Deserialize<Name>(data["name"]);
        var email = JsonSerializer.Deserialize<Email>(data["email"]);
        var type = JsonSerializer.Deserialize<UserType>(data["type"]);
        var companyCategory = JsonSerializer.Deserialize<CompanyCategory>(data["companyCategory"]);
        var companyName = JsonSerializer.Deserialize<Name>(data["companyName"]);
        var cnpj = JsonSerializer.Deserialize<CNPJ>(data["cnpj"]);

        if (name is null || email is null || type is null || companyCategory is null || cnpj is null) return;

        domainEvent.User
            .SetName(name)
            .SetEmail(email)
            .SetUserType(type);

        Vendor customer = Vendor.Create(domainEvent.User, companyCategory, name, cnpj);
        vendorRepository.Add(customer);

        await db.KeyDeleteAsync(key);
        await unitOfWork.SaveChangesAsync();
    }
}