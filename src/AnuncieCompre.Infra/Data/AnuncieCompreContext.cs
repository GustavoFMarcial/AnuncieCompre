using System.Text.Json;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.UseCase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContext(DbContextOptions<AnuncieCompreContext> options, IServiceProvider _serviceProvider) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    private readonly IServiceProvider serviceProvider = _serviceProvider;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conversation>(c =>
        {
            c.ComplexProperty(cp => cp.User);
        });

        modelBuilder.Entity<User>(u =>
        {
            u.ComplexProperty(cp => cp.Phone);
            u.OwnsOne(o => o.Name, name =>
            {
                name.Property(x => x.Value).IsRequired(false);
            });
            u.OwnsOne(o => o.Email, email =>
            {
                email.Property(x => x.Value).IsRequired(false);
            });
            // u.ComplexProperty(cp => cp.Name);
            // u.ComplexProperty(cp => cp.Email);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.ComplexProperty(cp => cp.User);
            o.ComplexProperty(cp => cp.Product);
            o.ComplexProperty(cp => cp.Quantity);
            o.ComplexProperty(cp => cp.Category);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            Type eventType = domainEvent.GetType();
            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            var handler = serviceProvider.GetRequiredService(handlerType);
            var method = handlerType.GetMethod("HandleAsync");

            if (method != null)
            {
                var taskResult = method.Invoke(handler, [domainEvent]);

                if (taskResult is Task task)
                {
                    await task;
                }
            }
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}