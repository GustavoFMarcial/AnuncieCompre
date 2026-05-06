using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentNameDomainEventHandler(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentNameDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentNameDomainEvent domainEvent)
    {
        Vendor? vendor = await vendorRepository.GetVendorByPhoneAsync(domainEvent.User.Phone.Value);

        if (vendor is null) return;
        
        vendor.SetName(domainEvent.Name);
        await unitOfWork.SaveChangesAsync();
    }
}