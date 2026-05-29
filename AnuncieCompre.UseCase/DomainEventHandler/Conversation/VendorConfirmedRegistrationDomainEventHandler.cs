using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorConfirmedRegistrationDomainEventHandler(IServiceProvider _serviceProvider, IDatabase _db) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:vendor-confirmed-registration", "workers", "vendor-confirmed-registration", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:vendor-confirmed-registration", "workers", "vendor-confirmed-registration", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();
                var repository = scope.ServiceProvider.GetRequiredService<IVendorRepository>();

                if (payload == null) return;

                var domainEvent = JsonSerializer.Deserialize<VendorConfirmedRegistrationDomainEvent>(payload);

                if (domainEvent == null) return;

                string key = $"user:{domainEvent.User.Phone.Value}";
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

                if (name is null || email is null || type is null || companyCategory is null || companyName is null || cnpj is null) return;

                domainEvent.User
                    .SetName(name)
                    .SetEmail(email)
                    .SetUserType(type);

                Vendor vendor = Vendor.Create(domainEvent.User, companyCategory, companyName, cnpj);
                repository.Add(vendor);

                await db.KeyDeleteAsync(key);
                await db.StreamAcknowledgeAsync("events:vendor-confirmed-registration", "workers", message.Id);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}