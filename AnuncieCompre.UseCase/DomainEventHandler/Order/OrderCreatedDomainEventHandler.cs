using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using System.Text.Json;
using AnuncieCompre.Infra.Data;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.OrderDomainEventHandler;


public class OrderCreatedDomainEventHandler(IServiceProvider _serviceProvider, IDatabase _db, IMessageSender _messageSender) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;
    private readonly IDatabase db = _db;
    private readonly IMessageSender messageSender = _messageSender;

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = await db.StreamReadGroupAsync("events:order-created", "workers", "order-created", "0-0", count: 5);

            if (messages.Length == 0)
            {
                messages = await db.StreamReadGroupAsync("events:order-created", "workers", "order-created", ">", count: 5);
            }

            foreach (var message in messages)
            {
                var eventId = (string?)message["eventId"];
                var payload = (string?)message["event"];
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();
                var vendorRepository = scope.ServiceProvider.GetRequiredService<IVendorRepository>();

                if (payload == null) continue;

                var domainEvent = JsonSerializer.Deserialize<OrderCreatedDomainEvent>(payload);

                if (domainEvent == null) continue;

                List<Vendor> vendors = await vendorRepository.GetVendorsByCategoryAsync((Domain.Enums.CompanyCategory)int.Parse(domainEvent.Category));

                foreach (Vendor v in vendors)
                {
                    try
                    {
                        await messageSender.SendMessageAsync(
                            v.User.Phone.Value,
                            $"""
                    Olá! Recebemos um pedido de {domainEvent.Quantity} de {domainEvent.Product} na sua região.

                    Gostaria de receber os detalhes para avaliar se consegue atender?
                    """);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Falha ao enviar mensagem via whatsapp para fornecedor {v.User.Phone.Value}");
                        Console.WriteLine(e.Message);
                    }
                }

                // await db.KeyDeleteAsync(key);
                await db.StreamAcknowledgeAsync("events:order-created", "workers", message.Id);
                await context.SaveChangesAsync(stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}