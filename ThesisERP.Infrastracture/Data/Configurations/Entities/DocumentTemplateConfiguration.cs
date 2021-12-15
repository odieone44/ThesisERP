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
    }
}
