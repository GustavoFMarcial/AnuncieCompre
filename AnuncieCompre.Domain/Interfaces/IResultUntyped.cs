namespace AnuncieCompre.Domain.Common;

public interface IResultUntyped
{
    public bool IsSuccess { get; }
    public string Message { get; }
    public object? Value { get; }
    public Type ValueType { get; }
}