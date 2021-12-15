using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class StockLevelConfiguration : IEntityTypeConfiguration<StockLevel>
{
    public void Configure(EntityTypeBuilder<StockLevel> stockLevelBuilder)
    {
        stockLevelBuilder.ToTable("StockLevels").HasKey(t => t.Id);
    }
}
