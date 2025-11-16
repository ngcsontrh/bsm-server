using BSM.Domain.Modules.IdentityModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BSM.Infrastructure.Persistent.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasOne<UserInformationEntity>(x => x.UserInformation)
            .WithOne()
            .HasForeignKey<UserInformationEntity>(x => x.AccountId);
    }
}