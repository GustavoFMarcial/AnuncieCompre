using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentDataToRegisterDomainEventHandler(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentDataToRegisterDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentDataToRegisterDomainEvent domainEvent)
    {
        Vendor vendor = Vendor.Create(domainEvent.User, domainEvent.CNPJ, domainEvent.CompanyCategory);
        vendorRepository.Add(vendor);

        await unitOfWork.SaveChangesAsync();
    }
}