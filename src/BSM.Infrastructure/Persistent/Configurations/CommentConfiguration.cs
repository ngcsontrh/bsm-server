using BSM.Domain.Modules.ContentModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasOne<BlogEntity>()
            .WithMany()
            .HasForeignKey(b => b.BlogId);
        
        builder.HasIndex(x => x.BlogId);
        builder.HasIndex(x => x.ParentId);
    }
}