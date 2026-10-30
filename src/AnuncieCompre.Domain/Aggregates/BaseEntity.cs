using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates;

public abstract class BaseEntity
{
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; private set; }

    public void UpdatedAt()
    {
        UpdateAt = DateTime.UtcNow;
    }
}