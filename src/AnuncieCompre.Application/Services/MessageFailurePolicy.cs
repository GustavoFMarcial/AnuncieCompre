using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Enums;
using Twilio.Exceptions;

namespace AnuncieCompre.Application.Services;

public class MessageFailurePolicy
{
    public MessageFailureType Classify(Exception exception)
    {
        MessageFailureType failureType;

        if (exception is ApiException e)
        {
            int[] retryableStatusCodes = [429, 500, 502, 503, 504];
            bool isRetryable = retryableStatusCodes.Any(c => c == e.Status);

            if (isRetryable)
            {
                failureType = MessageFailureType.Retryable;
            }
            
            failureType = MessageFailureType.Permanent;
        }
        else
        {
            failureType = exception switch
            {
                HttpRequestException => MessageFailureType.Retryable,
                TaskCanceledException => MessageFailureType.Retryable,
                OperationCanceledException => MessageFailureType.Retryable,
                _ => MessageFailureType.Permanent,
            };
        }

        return failureType;
    }
}