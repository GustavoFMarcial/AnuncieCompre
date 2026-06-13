using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerConfirmedRegistrationDomainEventHandler(IServiceProvider _serviceProvider, IDatabase _db) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-confirmed-registration", "workers", "customer-confirmed-registration", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-confirmed-registration", "workers", "customer-confirmed-registration", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var customerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<CustomerConfirmedRegistrationDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";
                var entries = await db.HashGetAllAsync(key);

                var data = entries.ToDictionary(
                    x => x.Name.ToString(),
                    x => x.Value.ToString()
                );

                var stringName = data["name"];
                var stringEmail = data["email"];
                var stringType = data["type"];
                var stringCPF = data["cpf"];

                if (stringName is null || stringEmail is null || stringType is null || stringCPF is null) continue;

                User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone);

                if (user is null) continue;

                Result<Name> name = Name.Create(stringName);
                Result<Email> email = Email.Create(stringEmail);
                Result<UserType> type = UserType.Create(stringType);
                Result<CPF> cpf = CPF.Create(stringCPF);

                user
                    .SetName(name.Value)
                    .SetEmail(email.Value)
                    .SetUserType(type.Value);

                Customer customer = Customer.Create(user, cpf.Value);
                customerRepository.Add(customer);

                // await db.KeyDeleteAsync(key);
                await db.StreamAcknowledgeAsync("events:customer-confirmed-registration", "workers", message.Id);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}