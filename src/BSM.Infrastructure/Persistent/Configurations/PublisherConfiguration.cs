using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<PublisherEntity>
{
    public void Configure(EntityTypeBuilder<PublisherEntity> builder)
    {
        builder.OwnsOne(x => x.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street).HasColumnName("Street");
            addressBuilder.Property(a => a.District).HasColumnName("District");
            addressBuilder.Property(a => a.Province).HasColumnName("Province");
            addressBuilder.Property(a => a.Country).HasColumnName("Country");
        });
    }
}