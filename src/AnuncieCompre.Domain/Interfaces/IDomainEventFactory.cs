using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;

namespace AnuncieCompre.Domain.Interfaces;

public interface IDomainEventFactory
{
    public IDomainEvent Handle(User user, ValueObject tempData);
}