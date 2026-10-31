using AnuncieCompre.Domain.Aggregates.ConversationAggregate;
using AnuncieCompre.Domain.Aggregates.MessageAggregate;
using Microsoft.EntityFrameworkCore;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.NodeAggregate;
using AnuncieCompre.Domain.Aggregates.MessageProviderReferenceAggregate;
using AnuncieCompre.Domain.Aggregates;
using AnuncieCompre.Domain.Aggregates.CustomerAggregate;
using Microsoft.AspNetCore.Identity;
using AnuncieCompre.Infra.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AnuncieCompre.Infra.Data;

public class AnuncieCompreContext(DbContextOptions<AnuncieCompreContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Conversation> Conversations { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<ConversationFlow> ConversationFlows { get; set; } = default!;
    public DbSet<ConversationNode> ConversationNodes { get; set; } = default!;
    public DbSet<MessageProviderReference> MessageProviderReferences { get; set; } = default!;
    public DbSet<Company> Companies { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(u =>
        {
            u.ComplexProperty(cp => cp.Phone);
            u.ComplexProperty(cp => cp.Name);
            u.ComplexProperty(cp => cp.Email);
        });

        modelBuilder.Entity<Conversation>(c =>
        {
            c.HasOne(c => c.Customer)
            .WithMany(c => c.Conversations);
            c.HasMany(cp => cp.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId);
        });

        modelBuilder.Entity<Message>(m =>
        {
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
            cn.HasMany(cn => cn.Transitions)
            .WithOne(t => t.ConversationNode)
            .HasForeignKey(t => t.ConversationNodeId)
            .OnDelete(DeleteBehavior.Cascade);
            cn.ComplexProperty(cn => cn.Position, b =>
            {
               b.Property(e => e.PositionX).HasColumnName("PositionX"); 
               b.Property(e => e.PositionY).HasColumnName("PositionY"); 
            });
        });

        modelBuilder.Entity<MessageProviderReference>(mpf =>
        {
            mpf.HasKey(x => x.Id);
            mpf.HasOne(x => x.Message)
                .WithOne()
                .HasForeignKey<MessageProviderReference>(x => x.MessageId);
            mpf.HasIndex(x => new { x.Provider, x.ProviderMessageId })
                .IsUnique();
        });

        modelBuilder.Entity<Company>(c =>
        {
            c.ComplexProperty(cp => cp.Name);
            c.ComplexProperty(cp => cp.Cnpj);
            c.ComplexProperty(cp => cp.Email);
            c.ComplexProperty(cp => cp.Phone);
        });

        modelBuilder.Entity<ApplicationUser>(au =>
        {
            au.ComplexProperty(cp => cp.Name);
            au.ComplexProperty(cp => cp.Cpf);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entities = ChangeTracker.Entries<BaseEntity>().Where(e => e.State != EntityState.Added);

        foreach (var e in entities)
        {
            e.Entity.UpdatedAt();
        }

        return await base.SaveChangesAsync(ct);
    }
}