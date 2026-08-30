using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Application.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerSentNameDomainEventHandler() : IDomainEventHandler<CustomerSentNameDomainEvent>
{

    public async Task HandleAsync(CustomerSentNameDomainEvent domainEvent)
    {
        domainEvent.Customer.SetName(domainEvent.Name);
    }
}