using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> documentBuilder)
        {
            documentBuilder.HasKey(e => e.Id);
            documentBuilder.ToTable("Documents");

            documentBuilder.HasOne(e => e.Entity)
                           .WithMany()
                           .HasForeignKey(d => d.EntityId);

            documentBuilder.HasOne(t => t.TransactionTemplate)
                           .WithMany()
                           .HasForeignKey(t => t.TemplateId);

            documentBuilder.HasOne(t => t.InventoryLocation)
                           .WithMany()
                           .HasForeignKey(t => t.InventoryLocationId);

            documentBuilder.Property(d => d.Timestamp).IsRowVersion();

            documentBuilder.OwnsMany(
                d => d.Details, detail =>
                {
                    detail.ToTable("DocumentDetails").HasKey(det => det.Id);
                    detail.WithOwner(x => x.ParentDocument).HasForeignKey(k => k.ParentDocumentId);
                    detail.HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
                    detail.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxID);
                    detail.HasOne(d => d.Discount).WithMany().HasForeignKey(d => d.DiscountID);
                    detail.Property(d => d.Timestamp).IsRowVersion();
                });

            documentBuilder.OwnsOne(s => s.ShippingAddress);
            documentBuilder.OwnsOne(b => b.BillingAddress);
        }
    }
}
