using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> orderBuilder)
    {
        orderBuilder.HasKey(e => e.Id);
        orderBuilder.ToTable("Orders");

        orderBuilder.HasOne(e => e.Entity)
                    .WithMany()
                    .HasForeignKey(d => d.EntityId);

        orderBuilder.HasOne(t => t.OrderTemplate)
                    .WithMany()
                    .HasForeignKey(t => t.TemplateId);

        //orderBuilder.HasOne(t => t.InventoryLocation)
        //               .WithMany()
        //               .HasForeignKey(t => t.InventoryLocationId);

        orderBuilder.Property(d => d.Timestamp).IsRowVersion();

        orderBuilder.OwnsMany(
            d => d.Rows, orderRow =>
            {
                orderRow.ToTable("OrderRows").HasKey(r => r.Id);
                orderRow.WithOwner(x => x.ParentOrder).HasForeignKey(k => k.ParentOrderId);
                orderRow.HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
                orderRow.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxID);
                orderRow.HasOne(d => d.Discount).WithMany().HasForeignKey(d => d.DiscountID);
                orderRow.Property(d => d.Timestamp).IsRowVersion();
            });

        orderBuilder.OwnsOne(s => s.ShippingAddress);
        orderBuilder.OwnsOne(b => b.BillingAddress);
    }
}
