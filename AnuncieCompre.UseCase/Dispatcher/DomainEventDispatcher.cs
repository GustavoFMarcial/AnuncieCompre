
using AnuncieCompre.Domain.Interfaces;
using AnuncieCompre.UseCase.Interfaces;

namespace AnuncieCompre.UseCase.Dispatcher;
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DomainEventDispatcher> _logger;
    
    public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var eventType = domainEvent.GetType();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        
        var handlers = _serviceProvider.GetServices(handlerType).ToList();
        
        if (handlers.Count == 0)
        {
            _logger.LogWarning(
                "Nenhum handler encontrado para o evento {EventType}", 
                eventType.Name);
            return;
        }
        
        _logger.LogInformation(
            "Despachando evento {EventType} para {HandlerCount} handler(s)",
            eventType.Name, 
            handlers.Count);
        
        foreach (var handler in handlers)
        {
            try
            {
                var method = handlerType.GetMethod(nameof(IDomainEventHandler<>.HandleAsync)) ?? throw new InvalidOperationException($"Método Handle não encontrado em {handler?.GetType().Name}");
                await (Task)method.Invoke(handler, [domainEvent])!;
                
                _logger.LogInformation(
                    "Handler {HandlerName} executado com sucesso para {EventType}", 
                    handler?.GetType().Name, 
                    eventType.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, 
                    "Erro ao executar handler {HandlerName} para evento {EventType}", 
                    handler?.GetType().Name, 
                    eventType.Name);
                
                // Você decide: lançar exceção ou continuar com próximo handler
                // throw;
            }
        }
    }
}