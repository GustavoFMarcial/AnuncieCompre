using System.Text.Json;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using StackExchange.Redis;

namespace AnuncieCompre.Application.BackgroundServices;

public class MessageRetry(IServiceProvider _serviceProvider) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<IDatabase>();
            var messageSender = scope.ServiceProvider.GetRequiredService<IMessageSender>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            RedisValue[] redisValues = await db.ListRangeAsync("messages:retry", 0, -1);
            List<Message> messages = [];

            foreach (RedisValue r in redisValues)
            {
                Message? message = JsonSerializer.Deserialize<Message>(r.ToString());
                await db.ListRemoveAsync("messages:retry", r);

                if (message is not null)
                {
                    message.IncrementRetryAttempts();
                    await messageSender.SendMessageAsync(message);
                }
            } 

            await unitOfWork.SaveChangesAsync();
            await Task.Delay(60000, stoppingToken);
        }
    }
}