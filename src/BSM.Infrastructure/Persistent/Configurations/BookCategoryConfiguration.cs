using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategoryEntity>
{
    public void Configure(EntityTypeBuilder<BookCategoryEntity> builder)
    {
        builder.HasKey(x => new { x.BookId, x.CategoryId });
        builder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId);
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}