using AnuncieCompre.Domain.Aggregates.ConversationAggregate;

namespace AnuncieCompre.UseCase.Services;

public class ValidateUserMessage
{
    public bool Handle(string[]? options, string message)
    {
        if (options is null) return true;
        else
        {
            foreach (string o in options)
            {
                if (message == o) return true;
            }
        }

        return false;
    }
}