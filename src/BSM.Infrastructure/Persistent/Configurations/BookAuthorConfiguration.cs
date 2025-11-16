using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthorEntity>
{
    public void Configure(EntityTypeBuilder<BookAuthorEntity> builder)
    {
        builder.HasKey(x => new { x.BookId, x.AuthorId });
        builder.HasOne(x => x.Book)
            .WithMany()
            .HasForeignKey(x => x.BookId);
        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId);
    }
}