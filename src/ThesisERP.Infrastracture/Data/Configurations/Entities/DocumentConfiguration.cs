using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> documentBuilder)
    {
        documentBuilder.HasKey(e => e.Id);
        documentBuilder.ToTable("Documents");

        documentBuilder.HasOne(e => e.Entity)
                       .WithMany()
                       .HasForeignKey(d => d.EntityId);

        documentBuilder.HasOne(t => t.Template)
                       .WithMany()
                       .HasForeignKey(t => t.TemplateId);

        documentBuilder.HasOne(t => t.InventoryLocation)
                       .WithMany()
                       .HasForeignKey(t => t.InventoryLocationId);

        documentBuilder.HasOne(o => o.ParentOrder)
                       .WithMany(d => d.RelatedDocuments)
                       .HasForeignKey(o => o.ParentOrderId);

        documentBuilder.Property(d => d.Timestamp).IsRowVersion();

        documentBuilder.OwnsMany(
            d => d.Rows, docRow =>
            {
                docRow.ToTable("DocumentRows").HasKey(r => r.Id);
                docRow.WithOwner(x => x.ParentDocument).HasForeignKey(k => k.ParentDocumentId);
                docRow.HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
                docRow.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxID);
                docRow.HasOne(d => d.Discount).WithMany().HasForeignKey(d => d.DiscountID);
                docRow.Property(d => d.Timestamp).IsRowVersion();
            });

        documentBuilder.OwnsOne(s => s.ShippingAddress);
        documentBuilder.OwnsOne(b => b.BillingAddress);
    }
}
