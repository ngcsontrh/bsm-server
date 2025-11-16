using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.Property(x => x.ISBN).IsUnicode(false);
        builder.ComplexProperty(x => x.CoverImage, x => x.ToJson("CoverImage"));
        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                .HasColumnName("PriceAmount");
            priceBuilder.Property(p => p.Currency)
                .HasColumnName("PriceCurrency");
        });
        
        builder.HasIndex(x => x.ISBN).IsUnique();
        builder.HasIndex(x => x.PublisherId);
    }
}