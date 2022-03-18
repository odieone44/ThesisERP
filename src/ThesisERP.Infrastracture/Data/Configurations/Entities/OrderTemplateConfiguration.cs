using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities;

internal class OrderTemplateConfiguration : IEntityTypeConfiguration<OrderTemplate>
{
    public void Configure(EntityTypeBuilder<OrderTemplate> templateBuilder)
    {
        templateBuilder.ToTable("OrderTemplates").HasKey(t => t.Id);
        templateBuilder.Property(t => t.Timestamp).IsRowVersion();
        templateBuilder.Property(t => t.Abbreviation).HasMaxLength(TransactionTemplateBase.AbbreviationMaxLength);

        templateBuilder.HasIndex(t => t.Abbreviation)
            .IncludeProperties(
                p => new {
                    p.Id,
                    p.Description,
                    p.OrderType,
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
