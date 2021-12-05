using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class InventoryLocationConfiguration : IEntityTypeConfiguration<InventoryLocation>
    {
        public void Configure(EntityTypeBuilder<InventoryLocation> locationBuilder) 
        {
            locationBuilder.ToTable("InventoryLocations")
                            .HasKey(x => x.Id);

            locationBuilder.OwnsOne(x => x.Address);

            locationBuilder.HasMany(s => s.StockLevels)
                            .WithOne(l => l.InventoryLocation)
                            .HasForeignKey(k=>k.InventoryLocationId);
        }
    }
}
