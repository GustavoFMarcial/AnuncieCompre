using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.Application.BackgroundServices;

public class CloseInactiveConversations(IServiceProvider _serviceProvider) : BackgroundService
{
    private readonly IServiceProvider serviceProvider = _serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var conversationRepository = scope.ServiceProvider.GetRequiredService<IConversationRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            List<Conversation> conversations = await conversationRepository.GetOpenConversationsAttendantByBotToListAsync();
            DateTime dateTimeNow = DateTime.UtcNow;

            foreach (Conversation c in conversations)
            {
                TimeSpan difference = dateTimeNow - c.DateTimeLastMessage;

                if (Math.Abs(difference.TotalMinutes) >= 30)
                {
                    c.Close();
                }
            }

            await unitOfWork.SaveChangesAsync();
            await Task.Delay(300000, stoppingToken);
        };
    }
}