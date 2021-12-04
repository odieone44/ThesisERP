using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class TransactionTemplateConfiguration : IEntityTypeConfiguration<TransactionTemplate>
    {
        public void Configure(EntityTypeBuilder<TransactionTemplate> templateBuilder)
        {
            templateBuilder.ToTable("TransactionTemplates").HasKey(t => t.Id);
            templateBuilder.Property(t => t.Timestamp).IsRowVersion();
        }
    }
}
