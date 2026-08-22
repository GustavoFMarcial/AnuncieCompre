using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using AnuncieCompre.Domain.Aggregates.OrderAggregate;
using AnuncieCompre.Domain.Aggregates.UserAggregate;
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
    public DbSet<ConversationFlow> ConversationFlows { get; set; } = default!;
    public DbSet<ConversationNode> ConversationNodes { get; set; } = default!;

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
            m.ComplexProperty(cp => cp.Conversation);
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.HasOne(cp => cp.User);
            o.ComplexProperty(cp => cp.Product);
            o.ComplexProperty(cp => cp.Quantity);
            o.ComplexProperty(cp => cp.Category);
        });

        modelBuilder.Entity<ConversationFlow>(cf =>
        {
            cf.ComplexProperty(cp => cp.Name);
            cf.HasMany(f => f.Nodes)
            .WithOne(n => n.ConversationFlow)
            .HasForeignKey(n => n.ConversationFlowId)
            .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ConversationNode>(cn =>
        {
            cn.ComplexCollection(cp => cp.Transitions).ToJson();
        });
    }
}