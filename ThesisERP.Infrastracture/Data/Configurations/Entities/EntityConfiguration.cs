using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entites;

namespace ThesisERP.Infrastracture.Data.Configurations.Entities
{
    internal class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        public void Configure(EntityTypeBuilder<Entity> entityBuilder)
        {

            entityBuilder.ToTable("Entities");
            entityBuilder.HasKey(t => t.Id);

            entityBuilder.Property(e => e.Timestamp).IsRowVersion();

            entityBuilder.HasMany(e => e.RelatedProducts)
                         .WithMany(x => x.RelatedEntities);            

            entityBuilder.OwnsMany(a => a.EntityAdresses)
                         .ToTable("EntityAddresses")
                         .WithOwner(e => e.Entity)
                         .HasForeignKey(e => e.EntityId);

            entityBuilder.OwnsMany(a => a.EntityAdresses).HasKey(e => e.Id);
        }
    }
}
