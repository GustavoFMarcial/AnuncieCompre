using AnuncieCompre.Domain.Aggregates.OrderAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.DomainEventHandler.OrderDomainEventHandler;

public class OrderCreatedDomainEventHandler(IMessageSender _messageSender, IVendorRepository _vendorRepository) : IDomainEventHandler<OrderCreatedDomainEvent>
{
    private readonly IMessageSender messageSender = _messageSender;
    private readonly IVendorRepository vendorRepository = _vendorRepository;

    public async Task HandleAsync(OrderCreatedDomainEvent domainEvent)
    {
        List<Vendor> vendors = await vendorRepository.GetVendorsByCategoryAsync(domainEvent.Category);

        foreach (Vendor v in vendors)
        {
            try
            {
                await messageSender.SendMessageAsync(
                    v.User.Phone.Value,
                    $"""
                    Olá! Recebemos um pedido de {domainEvent.Quantity.Value} de {domainEvent.Product.Value} na sua região.

                    Gostaria de receber os detalhes para avaliar se consegue atender?
                    """);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Falha ao enviar mensagem via whatsapp para {domainEvent.UserPhone.Value}");
                Console.WriteLine(e.Message);
            }
        }
    }
}