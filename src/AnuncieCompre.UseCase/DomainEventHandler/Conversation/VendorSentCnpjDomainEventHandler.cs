using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class VendorSentCnpjDomainEventHandler(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentCnpjDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentCnpjDomainEvent domainEvent)
    {
        Vendor? vendor = await vendorRepository.GetVendorByPhoneAsync(domainEvent.Phone.Value);

        if (vendor is null) return;

        vendor.SetCNPJ(domainEvent.Cnpj);
        await unitOfWork.SaveChangesAsync();
    }
}

