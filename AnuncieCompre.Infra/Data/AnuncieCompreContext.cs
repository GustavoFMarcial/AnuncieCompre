using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Dispatcher;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContext(DbContextOptions<AnuncieCompreContext> options, IDomainEventDispatcher _domainEventDispatcher) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    private readonly IDomainEventDispatcher domainEventDispatcher = _domainEventDispatcher;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>(c =>
        {
            c.ComplexProperty(cp => cp.UserPhone);
            c.ComplexProperty(cp => cp.TempData).ToJson();
        });

        modelBuilder.Entity<User>(u =>
        {
            u.ComplexProperty(cp => cp.Phone);
            u.ComplexProperty(cp => cp.Name);
            u.ComplexProperty(cp => cp.Email);
        });

        modelBuilder.Entity<Customer>(s =>
        {
            s.ComplexProperty(cp => cp.CPF);
        });

        modelBuilder.Entity<Vendor>(s =>
        {
            s.ComplexProperty(cp => cp.CNPJ);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.ComplexProperty(cp => cp.UserPhone);
            o.ComplexProperty(cp => cp.Product);
            o.ComplexProperty(cp => cp.Quantity);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await domainEventDispatcher.DispatchAsync(domainEvent, cancellationToken);
        }

        return result;
    }
}