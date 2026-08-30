using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Exceptions;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerSentCompanyCategoryDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Customer customer, ValueObject data)
    {
        
        if (data is CompanyCategory companyCategory)
        {
            return new CustomerSentCompanyCategoryDomainEvent(customer, companyCategory);
        }

        throw new DomainException("Tipo inválido do ValueObject");
    }
}