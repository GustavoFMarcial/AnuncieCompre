using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.ConversationDomainEventHandler;

public class VendorSentDataToRegisterDomainEventHandler(IUserRepository _userRepository, IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentDataToRegisterDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentDataToRegisterDomainEvent domainEvent)
    {
        User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone!.Value);

        Vendor vendor = Vendor.Create(user!, domainEvent.CNPJ, domainEvent.CompanyCategory);
        vendorRepository.Add(vendor);

        await unitOfWork.SaveChangesAsync();
    }
}