using BSM.Domain.Modules.IdentityModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class UserInformationConfiguration : IEntityTypeConfiguration<UserInformationEntity>
{
    public void Configure(EntityTypeBuilder<UserInformationEntity> builder)
    {
        builder.OwnsOne(x => x.Code, codeBuilder =>
        {
            codeBuilder.Property(x => x.Value).HasColumnName("Code").IsUnicode(false);
            codeBuilder.HasIndex(x => x.Value).IsUnique();
        });
        builder.OwnsOne(x => x.IdentityNumber, identityNumberBuilder =>
        {
            identityNumberBuilder.Property(x => x.Value).HasColumnName("IdentityNumber").IsUnicode(false);
        });
        builder.HasIndex(x => x.AccountId).IsUnique();
    }
}