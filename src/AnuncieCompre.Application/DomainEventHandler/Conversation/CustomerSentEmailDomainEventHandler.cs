using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Application.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.Application.DomainEventHandler.Conversation;

public class CustomerSentEmailDomainEventHandler() : IDomainEventHandler<CustomerSentEmailDomainEvent>
{

    public async Task HandleAsync(CustomerSentEmailDomainEvent domainEvent)
    {
        domainEvent.Customer.SetEmail(domainEvent.Email);
    }
}