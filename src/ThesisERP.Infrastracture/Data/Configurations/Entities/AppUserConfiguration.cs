using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> appUserBuilder)
    {
        appUserBuilder
            .OwnsMany(
                t => t.RefreshTokens, token =>
                {
                    token.ToTable("RefreshTokens").HasKey(u => u.Id);
                    token.WithOwner().HasForeignKey(u => u.UserId);
                });
    }
}
