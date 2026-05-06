using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentCompanyCategoryDomainEventHandler(IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentCompanyCategoryDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentCompanyCategoryDomainEvent domainEvent)
    {
        Vendor? vendor = Vendor.Create(domainEvent.User, domainEvent.CompanyCategory);
        
        vendorRepository.Add(vendor);
        await unitOfWork.SaveChangesAsync();
    }
}