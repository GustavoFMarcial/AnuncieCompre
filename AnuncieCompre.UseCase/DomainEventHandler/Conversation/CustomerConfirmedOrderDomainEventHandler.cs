using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Infra.Data;
using AnuncieCompre.Infra.Repositories.CustomerRepo;
using AnuncieCompre.Infra.Repositories.OrderRepo;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class CustomerConfirmedOrderDomainEventHandler(IServiceProvider _serviceProvider, IDatabase _db) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;
    private readonly IDatabase db = _db;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:customer-confirmed-order", "workers", "customer-confirmed-order", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:customer-confirmed-order", "workers", "customer-confirmed-order", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<CustomerConfirmedOrderDomainEvent>(payload);

                if (domainEvent == null) continue;

                string key = $"session:{domainEvent.Phone}";
                var entries = await db.HashGetAllAsync(key);

                var data = entries.ToDictionary(
                    x => x.Name.ToString(),
                    x => x.Value.ToString()
                );

                var stringCompanyCategory = data["companyCategory"];
                var stringProduct = data["product"];
                var stringQuantity = data["quantity"];

                if (stringCompanyCategory is null || stringProduct is null || stringQuantity is null) continue;

                Result<Phone> phone = Phone.Create(domainEvent.Phone);
                Result<CompanyCategory> companyCategory = CompanyCategory.Create(stringCompanyCategory);
                Result<Product> product = Product.Create(stringProduct);
                Result<Quantity> quantity = Quantity.Create(stringQuantity);

                Domain.Aggregates.OrderAggregate.Order order = Domain.Aggregates.OrderAggregate.Order.Create(phone.Value, product.Value, quantity.Value, companyCategory.Value);
                orderRepository.Add(order);

                // await db.KeyDeleteAsync(key);
                await db.StreamAcknowledgeAsync("events:customer-confirmed-order", "workers", message.Id);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}