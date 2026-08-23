namespace AnuncieCompre.Domain.Common;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public string Message  { get; protected set; } = default!;

    protected Result(){}

    public static Result Success(string message)
    {
        Result result = new()
        {
            IsSuccess = true,
            Message = message,
        };

        return result;
    }

    public static Result Failure(string message)
    {
        Result result = new()
        {
            IsSuccess = false,
            Message = message,
        };

        return result;
    }
}