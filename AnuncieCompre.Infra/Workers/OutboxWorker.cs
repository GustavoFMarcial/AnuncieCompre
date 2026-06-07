using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.OutOfBoxAggregate;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AnuncieCompre.Infra.Workers;

public class OutboxWorker(IServiceProvider _serviceProvider, IDatabase _db) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;
    private readonly IDatabase db = _db;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AnuncieCompreContext>();

            List<OutboxMessage> messages = await context.Set<OutboxMessage>().Where(o => !o.IsProcessed).ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                var values = new NameValueEntry[]
                {
                    new("eventId", message.Id.ToString()),
                    new("event", message.PayloadJson),
                };

                await db.StreamAddAsync($"events:{message.EventType}", values, messageId: "*");

                message.IsProcessed = true;
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}