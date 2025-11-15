using BSM.Domain.Modules.BookModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class PublisherConfiguration : IEntityTypeConfiguration<PublisherEntity>
{
    public void Configure(EntityTypeBuilder<PublisherEntity> builder)
    {
        builder.ComplexProperty(x => x.Address, x => x.ToJson());
    }
}