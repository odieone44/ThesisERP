using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> productBuilder)
    {
        productBuilder.ToTable("Products").HasKey(t => t.Id);
        productBuilder.Property(p => p.Timestamp).IsRowVersion();

        productBuilder.Property(p => p.SKU).HasMaxLength(40).IsRequired();
        productBuilder.Property(p => p.Description).HasMaxLength(200).IsRequired();
        productBuilder.Property(p => p.LongDescription).HasMaxLength(4000);
        //productBuilder.Property(p => p.Type).HasDefaultValue(Products.Types.product);


        productBuilder.HasMany(s => s.StockLevels)
                      .WithOne(l => l.Product)
                      .HasForeignKey(k => k.ProductId);
    }
}
