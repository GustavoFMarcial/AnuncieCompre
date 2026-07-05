namespace AnuncieCompre.Domain.Common;

public class Result<T> : IResultValueObject where T : ValueObject
{
    public bool IsSuccess { get; protected set; }
    public string Message  { get; protected set; } = default!;
    public T Value { get; protected set; } = default!;
    ValueObject? IResultValueObject.Value => Value;

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

    public static Result<T> Failure(string message)
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