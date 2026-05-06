using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCnpjDomainEventFactory(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentCnpjDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentCnpjDomainEvent domainEvent)
    {
        Vendor? vendor = await vendorRepository.GetVendorByPhoneAsync(domainEvent.User.Phone.Value);

        if (vendor is null) return;
        
        vendor.SetCNPJ(domainEvent.CNPJ);
        await unitOfWork.SaveChangesAsync();
    }
}