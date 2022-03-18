using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class DocumentTemplateConfiguration : IEntityTypeConfiguration<DocumentTemplate>
{
    public void Configure(EntityTypeBuilder<DocumentTemplate> templateBuilder)
    {
        templateBuilder.ToTable("DocumentTemplates").HasKey(t => t.Id);
        templateBuilder.Property(t => t.Timestamp).IsRowVersion();
        templateBuilder.Property(t => t.Abbreviation).HasMaxLength(TransactionTemplateBase.AbbreviationMaxLength);

        templateBuilder.HasIndex(t => t.Abbreviation)
            .IncludeProperties(
                p => new {
                    p.Id,
                    p.Description,
                    p.DocumentType,
                    p.IsDeleted,
                    p.Name,
                    p.NextNumber,
                    p.Prefix,
                    p.Postfix,
                    p.DateCreated,
                    p.DateUpdated,
                    p.Timestamp
                })
            .IsUnique(); ;
    }
}
