using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.OwnsOne(x => x.ISBN, isbnBuilder =>
        {
            isbnBuilder.Property(i => i.Value)
                .HasColumnName("ISBN")
                .IsUnicode(false);
            isbnBuilder.HasIndex(i => i.Value);
        });
        builder.ComplexProperty(x => x.CoverImage, x => x.ToJson());
        builder.OwnsOne(x => x.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                .HasColumnName("Price");
            priceBuilder.Property(p => p.CurrencyCode)
                .HasColumnName("PriceCurrencyCode");
        });
    }
}