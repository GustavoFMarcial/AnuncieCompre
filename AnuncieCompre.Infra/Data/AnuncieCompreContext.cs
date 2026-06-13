using System.Text.Json;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.OutOfBoxAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContext(DbContextOptions<AnuncieCompreContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Vendor> Vendors { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OutboxMessage> OutBoxMessage { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>(c =>
        {
            c.ComplexProperty(cp => cp.UserPhone);
        });

        modelBuilder.Entity<User>(u =>
        {
            u.ComplexProperty(cp => cp.Phone);
            u.ComplexProperty(cp => cp.Type);
            u.OwnsOne(o => o.Name, name =>
            {
                name.Property(x => x.Value).IsRequired(false);
            });
            u.OwnsOne(o => o.Email, email =>
            {
                email.Property(x => x.Value).IsRequired(false);
            });
        });

        modelBuilder.Entity<Customer>(s =>
        {
            s.ComplexProperty(cp => cp.CPF);
        });

        modelBuilder.Entity<Vendor>(s =>
        {
            s.ComplexProperty(cp => cp.Name);
            s.ComplexProperty(cp => cp.CNPJ);
            s.ComplexProperty(cp => cp.Category);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.ComplexProperty(cp => cp.UserPhone);
            o.ComplexProperty(cp => cp.Product);
            o.ComplexProperty(cp => cp.Quantity);
            o.ComplexProperty(cp => cp.Category);
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

        foreach (var domainEvent in domainEvents)
        {
            Add(new OutboxMessage
            {
                EventType = domainEvent.EventType,
                PayloadJson = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            });
        }

        var result = await base.SaveChangesAsync(cancellationToken);
        
        return result;
    }
}