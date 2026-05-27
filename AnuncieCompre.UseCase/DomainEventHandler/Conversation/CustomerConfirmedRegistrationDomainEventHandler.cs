using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerConfirmedRegistrationDomainEventHandler(IServiceProvider _serviceProdiver, IDatabase _db/*, ICustomerRepository _customerRepository, IUnitOfWork _unitOfWork*/) : BackgroundService/*, IDomainEventHandler<CustomerConfirmedRegistrationDomainEvent>*/
{
    private readonly IServiceProvider serviceProvider = _serviceProdiver;
    private readonly IDatabase db = _db;
    // private readonly ICustomerRepository customerRepository = _customerRepository;
    // private readonly IUnitOfWork unitOfWork = _unitOfWork;

    // public async Task HandleAsync(CustomerConfirmedRegistrationDomainEvent domainEvent)
    // {
    //     string key = $"user:{domainEvent.User.Phone.Value}";
    //     var entries = await db.HashGetAllAsync(key);

    //     var data = entries.ToDictionary(
    //         x => x.Name.ToString(),
    //         x => x.Value.ToString()
    //     );

    //     var name = JsonSerializer.Deserialize<Name>(data["name"]);
    //     var email = JsonSerializer.Deserialize<Email>(data["email"]);
    //     var type = JsonSerializer.Deserialize<UserType>(data["type"]);
    //     var cpf = JsonSerializer.Deserialize<CPF>(data["cpf"]);

    //     if (name is null || email is null || type is null || cpf is null) return;

    //     domainEvent.User
    //         .SetName(name)
    //         .SetEmail(email)
    //         .SetUserType(type);

    //     Customer customer = Customer.Create(domainEvent.User, cpf);
    //     customerRepository.Add(customer);

    //     await db.KeyDeleteAsync(key);
    //     await unitOfWork.SaveChangesAsync();
    // }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync($"events:customer-confirmed-registration", "workers", "customer-confirmed-registration", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync($"events:customer-confirmed-registration", "workers", "customer-confirmed-registration", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                using var scope = serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();
                var repository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();

                if (payload == null) return;

                var domainEvent = JsonSerializer.Deserialize<CustomerConfirmedRegistrationDomainEvent>(payload);

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
                var cpf = JsonSerializer.Deserialize<CPF>(data["cpf"]);

                if (name is null || email is null || type is null || cpf is null) return;

                domainEvent.User
                    .SetName(name)
                    .SetEmail(email)
                    .SetUserType(type);

                Customer customer = Customer.Create(domainEvent.User, cpf);
                repository.Add(customer);

                await db.KeyDeleteAsync(key);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}