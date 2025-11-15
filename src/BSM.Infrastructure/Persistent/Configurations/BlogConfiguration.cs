using BSM.Domain.Modules.ContentModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.OwnsOne(x => x.Status, statusBuilder =>
        {
            statusBuilder.Property(x => x.Value).HasColumnName("Status");
        });
    }
}