using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entites;

namespace ThesisERP.Infrastracture.Configurations.Entities
{
    internal class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> discountBuilder)
        {
            discountBuilder.ToTable("Discounts").HasKey(t => t.Id);
        }
    }
}
