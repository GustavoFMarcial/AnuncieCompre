using System.Text.Json;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using StackExchange.Redis;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class VendorSentCompanyCategoryDomainEventHandler(IUserRepository _userRepository, IVendorRepository _vendorRepository, IUnitOfWork _unitOfWork) : IDomainEventHandler<VendorSentCompanyCategoryDomainEvent>
{
    private readonly IUserRepository userRepository = _userRepository;
    private readonly IVendorRepository vendorRepository = _vendorRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task HandleAsync(VendorSentCompanyCategoryDomainEvent domainEvent)
    {
        User? user = await userRepository.GetUserByPhoneAsync(domainEvent.Phone.Value);

        if (user is null) return;

        Vendor vendor = Vendor.Create(user, domainEvent.CompanyCategory);

        vendorRepository.Add(vendor);
        await unitOfWork.SaveChangesAsync();
    }
}