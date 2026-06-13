using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;

public class CustomerConfirmedOrderDomainEvent(string phone) : IDomainEvent
{
    public string Phone { get; set; } = phone;
    public string EventType { get; private set; } = "customer-confirmed-order";
}