using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; private set; }
    private readonly List<IDomainEvent> domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }

    public void UpdatedAt()
    {
        UpdateAt = DateTime.UtcNow;
    }
}