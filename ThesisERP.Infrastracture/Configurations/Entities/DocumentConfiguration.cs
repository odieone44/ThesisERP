using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entites;

namespace ThesisERP.Infrastracture.Configurations.Entities
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

            documentBuilder.Property(d => d.Timestamp).IsRowVersion();

            documentBuilder.OwnsMany(d => d.Details)
                           .WithOwner(x => x.ParentDocument)
                           .HasForeignKey(k => k.ParentDocumentId);

            documentBuilder.OwnsMany(d => d.Details)
                           .ToTable("DocumentDetails").HasKey(det => det.Id);

            documentBuilder.OwnsMany(d => d.Details).HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
            documentBuilder.OwnsMany(d => d.Details).HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxID);
            documentBuilder.OwnsMany(d => d.Details).HasOne(d => d.Discount).WithMany().HasForeignKey(d => d.DiscountID);

            documentBuilder.OwnsMany(d => d.Details).Property(d => d.Timestamp).IsRowVersion();

            documentBuilder.OwnsMany(d => d.DocumentAddresses)
                           .ToTable("DocumentAddresses")
                           .WithOwner(a => a.Document)
                           .HasForeignKey(d => d.DocumentId);

            documentBuilder.OwnsMany(d => d.DocumentAddresses).HasKey(d => d.Id);
        }
    }
}
