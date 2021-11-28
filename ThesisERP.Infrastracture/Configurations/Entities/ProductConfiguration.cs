using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entites;

namespace ThesisERP.Infrastracture.Configurations.Entities
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> productBuilder)
        {
            productBuilder.ToTable("Products").HasKey(t => t.Id);
            productBuilder.Property(p => p.Timestamp).IsRowVersion();
        }
    }
}
