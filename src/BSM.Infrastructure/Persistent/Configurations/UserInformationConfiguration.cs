using BSM.Domain.Modules.IdentityModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class UserInformationConfiguration : IEntityTypeConfiguration<UserInformationEntity>
{
    public void Configure(EntityTypeBuilder<UserInformationEntity> builder)
    {
        builder.Property(x => x.Code).IsUnicode(false);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.AccountId);
    }
}