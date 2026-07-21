using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class VendorSentCompanyNameDomainEventHandler(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentCompanyNameDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentCompanyNameDomainEvent domainEvent)
    {
        Vendor? vendor = await vendorRepository.GetVendorByPhoneAsync(domainEvent.Phone.Value);

        if (vendor is null) return;

        vendor.SetName(domainEvent.Name);
        await unitOfWork.SaveChangesAsync();
    }
}

