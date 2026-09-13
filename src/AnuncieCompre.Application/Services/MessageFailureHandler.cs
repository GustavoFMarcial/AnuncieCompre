using System.Text.Json;
using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using StackExchange.Redis;


namespace AnuncieCompre.Application.Services;

public class MessageFailureHandler(MessageFailurePolicy _messageFailurePolicy, IDatabase _db, IUnitOfWork _unitOfWork)
{
    private readonly MessageFailurePolicy messageFailurePolicy = _messageFailurePolicy;
    private readonly IDatabase db = _db;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task Handle(Exception exception, Message message)
    {
        if (message.FailureType is MessageFailureType.Permanent) return;
        if (message.RetryAttempts >= message.MaxRetryAttempts)
        {
            message.SetFailureType(MessageFailureType.Permanent);
            return;
        }

        MessageFailureType failureType = messageFailurePolicy.Classify(exception);
        message.SetFailureType(failureType);

        if (failureType is MessageFailureType.Permanent) return;

        await db.ListLeftPushAsync("messages:retry", [JsonSerializer.Serialize(message)]);
        await unitOfWork.SaveChangesAsync();
    }
}