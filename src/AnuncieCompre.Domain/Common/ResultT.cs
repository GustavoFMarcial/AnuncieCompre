using AnuncieCompre.Domain.Aggregates;

namespace AnuncieCompre.Domain.Common;

public class Result<T> : Result
{
    public T Value { get; protected set; } = default!;

    protected Result(){}

    public static Result<T> Success(T value, string message)
    {
        Result<T> result = new()
        {
            IsSuccess = true,
            Message = message,
            Value = value,
        };

        return result;
    }

    public new static Result<T> Failure(string message)
    {
        Result<T> result = new()
        {
            IsSuccess = false,
            Message = message,
            Value = default!,
        };

        return result;
    }
}