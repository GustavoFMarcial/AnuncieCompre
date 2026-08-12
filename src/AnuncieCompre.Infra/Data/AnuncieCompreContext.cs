using System.Text.Json;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.ConversationAggregate.DomainEvents;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
using AnuncieCompre.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContext(DbContextOptions<AnuncieCompreContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<ConversationFlow> Flows { get; set; } = default!;
    public DbSet<ConversationNode> Nodes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(u =>
        {
            u.ComplexProperty(cp => cp.Phone);
            u.ComplexProperty(cp => cp.Name);
            u.ComplexProperty(cp => cp.Email);
        });

        modelBuilder.Entity<Conversation>(c =>
        {
            c.ComplexProperty(cp => cp.User);
        });

        modelBuilder.Entity<Message>(m =>
        {
            m.ComplexProperty(m => m.Conversation);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.HasOne(cp => cp.User);
            o.ComplexProperty(cp => cp.Product);
            o.ComplexProperty(cp => cp.Quantity);
            o.ComplexProperty(cp => cp.Category);
        });
    }
}