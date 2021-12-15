using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> entityBuilder)
    {

        entityBuilder.ToTable("Entities");
        entityBuilder.HasKey(t => t.Id);

        entityBuilder.Property(e => e.Timestamp).IsRowVersion();

        entityBuilder.HasMany(e => e.RelatedProducts)
                     .WithMany(x => x.RelatedEntities);

        entityBuilder.OwnsOne(s => s.ShippingAddress);
        entityBuilder.OwnsOne(b => b.BillingAddress);

    }
}
