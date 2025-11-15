using BSM.Domain.Modules.ContentModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder.OwnsOne(x => x.Status, statusBuilder =>
        {
            statusBuilder.Property(s => s.Value) .HasColumnName("Status");
        });
    }
}