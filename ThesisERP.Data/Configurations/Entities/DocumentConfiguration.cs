using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Data.Configurations.Entities
{
    internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> documentBuilder)
        {
            documentBuilder.HasOne(e => e.Entity);
            
            documentBuilder.HasOne(t => t.TransactionTemplate);


            documentBuilder.OwnsMany(d => d.Details)
                           .WithOwner(x => x.ParentDocument)
                           .HasForeignKey(k => k.ParentDocumentId);

            documentBuilder.OwnsMany(d => d.Details)
                           .Property(e => e.LineTotalNet)
                           .UsePropertyAccessMode(PropertyAccessMode.Property);

            documentBuilder.OwnsMany(d => d.DocumentAddresses)
                           .WithOwner(a => a.Document)
                           .HasForeignKey(d => d.DocumentId);
        }
    }
}
