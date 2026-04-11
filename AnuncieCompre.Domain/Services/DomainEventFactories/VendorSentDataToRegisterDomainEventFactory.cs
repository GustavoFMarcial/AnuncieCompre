using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.Common;
using AnuncieCompre.Domain.Interfaces;

namespace AnuncieCompre.Domain.Services.DomainEventFactories;

public class VendorSentDataToRegisterDomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Handle(Phone userPhone, Dictionary<string, ValueObject> tempData)
    {
        if (tempData["CompanyCategory"] is CompanyCategory companyCategory && tempData["CNPJ"] is CNPJ cnpj)
        {
            return new VendorSentDataToRegisterDomainEvent(userPhone, companyCategory, cnpj);
        }

        throw new NotImplementedException();
    }
}