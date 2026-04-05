namespace AnuncieCompre.Domain.Common;

public class Result<T> : IResultValueObject where T : ValueObject
{
    public bool IsSuccess { get; private set; }
    public string Message  { get; private set; } = default!;
    public T Value { get; private set; } = default!;
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