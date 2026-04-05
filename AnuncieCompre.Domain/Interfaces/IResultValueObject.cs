namespace AnuncieCompre.Domain.Common;

public interface IResultValueObject
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public ValueObject? Value { get; }
}