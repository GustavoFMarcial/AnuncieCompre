using AnuncieCompre.Domain.Aggregates.OutOfBoxAggregate;
using AnuncieCompre.Infra.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AnuncieCompre.Infra.Workers;

public class OutboxWorker(IDatabase _db) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = default!;
    private readonly IDatabase db = _db;
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var service = serviceProvider.GetRequiredService<AnuncieCompreContext>();

        while (stoppingToken.IsCancellationRequested)
        {
            List<OutboxMessage> outboxMessages = await service.Set<OutboxMessage>().Where(o => o.IsProcessed == false).ToListAsync(cancellationToken: stoppingToken);

            foreach (OutboxMessage o in outboxMessages)
            {
                try
                {
                    await db.StreamCreateConsumerGroupAsync($"events:{o.EventType}", "workers", "$", createStream: true);
                }
                catch (RedisServerException ex) when (ex.Message.StartsWith("BUSYGROUP")){}
            }
        }

        throw new NotImplementedException();
    }
}