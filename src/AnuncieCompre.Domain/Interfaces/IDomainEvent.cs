namespace AnuncieCompre.Domain.Interfaces;

public interface IDomainEvent
{
    public string EventType { get; }
}