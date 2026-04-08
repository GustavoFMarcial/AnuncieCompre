using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class CustomerSentDataToOrderDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Phone userPhone, Dictionary<string, ValueObject> tempData)
    {
        
        if (tempData["Product"] is Product product && tempData["Quantity"] is Quantity quantity && tempData["CompanyCategory"] is CompanyCategory companyCategory)
        {
            var domainevent = CustomerSentDataToOrderDomainEvent.Create(userPhone, product, quantity, companyCategory);
        }

        throw new NotImplementedException();
    }
}