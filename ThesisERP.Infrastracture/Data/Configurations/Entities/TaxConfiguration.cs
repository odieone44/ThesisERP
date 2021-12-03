using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entites;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> taxBuilder)
        {
            taxBuilder.ToTable("Taxes").HasKey(t => t.Id);
        }
    }
}
