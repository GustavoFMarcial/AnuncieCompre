using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Interfaces;

public interface IDomainEventFactory
{
    public IDomainEvent Handle(Phone userPhone, Dictionary<string, ValueObject> tempData);
}