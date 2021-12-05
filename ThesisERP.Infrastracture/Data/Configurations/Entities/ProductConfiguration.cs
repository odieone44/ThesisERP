using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> productBuilder)
        {
            productBuilder.ToTable("Products").HasKey(t => t.Id);
            productBuilder.Property(p => p.Timestamp).IsRowVersion();

            productBuilder.HasMany(s => s.StockLevels)
                          .WithOne(l => l.Product)
                          .HasForeignKey(k => k.ProductId);            
        }
    }
}
