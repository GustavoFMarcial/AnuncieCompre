using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.Conversation;

public class VendorDoesNotConfirmedRegistrationDomainEventHandler(IVendorRepository _vendorRepository) : IDomainEventHandler<VendorDoesNotConfirmedRegistrationDomainEvent>
{
    private readonly IVendorRepository vendorRepository = _vendorRepository;

    public async Task HandleAsync(VendorDoesNotConfirmedRegistrationDomainEvent domainEvent)
    {
        await vendorRepository.ExecuteDeleteAsync(domainEvent.Phone.Value);
    }
}