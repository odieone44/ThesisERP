using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Data.Configurations.Entities
{
    internal class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> entityBuilder)
        {
            entityBuilder.HasMany(e => e.RelatedProducts)
                         .WithMany(x => x.RelatedEntities);

            entityBuilder.OwnsMany(a => a.EntityAdresses)
                         .WithOwner(e => e.Entity)
                         .HasForeignKey(e => e.EntityId);
        }
    }
}
