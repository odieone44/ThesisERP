using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> appUserBuilder)
        {
            appUserBuilder
                .OwnsMany(t => t.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey(u => u.UserId);

            appUserBuilder
                .OwnsMany(t => t.RefreshTokens).HasKey(u => u.Id);
        }
    }
}
