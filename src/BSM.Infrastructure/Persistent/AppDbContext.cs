using System.Reflection;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Modules.BookModule.Entities;
using BSM.Domain.Modules.ContentModule.Entities;
using BSM.Domain.Modules.IdentityModule.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSM.Infrastructure.Persistent;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AccountEntity, RoleEntity, Guid>(options)
{
    public DbSet<AuthorEntity> Authors => Set<AuthorEntity>();
    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<PublisherEntity> Publishers => Set<PublisherEntity>();
    public DbSet<BlogEntity> Blogs => Set<BlogEntity>();
    public DbSet<CommentEntity> Comments => Set<CommentEntity>();
    public DbSet<EventEntity> Events => Set<EventEntity>();
    public DbSet<UserInformationEntity> UserInformations => Set<UserInformationEntity>();
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        var relationships = builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        var entityTypes = builder.Model.GetEntityTypes()
            .Where(x => x.ClrType.IsSubclassOf(typeof(EntityBase)));
        foreach (var entityType in entityTypes)
        {
            builder.Entity(entityType.ClrType)
                .Ignore(nameof(EntityBase.DomainEvents));
        }
    }
}