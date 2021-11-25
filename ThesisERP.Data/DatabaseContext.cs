using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Data.Configurations.Entities;

namespace ThesisERP.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Entity> Entities { get; set; }    
        public DbSet<Document> Documents { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var property in builder.Model.GetEntityTypes()
                                                   .SelectMany(t => t.GetProperties())
                                                   .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(6);
            }

            builder.Entity<AppUser>().OwnsMany(t => t.RefreshTokens);
            builder.Entity<Document>(d =>
            {
                d.HasOne(e => e.Entity);
                d.HasOne(t => t.TransactionTemplate);


                d.OwnsMany(d => d.Details)
                 .WithOwner(x => x.ParentDocument)
                 .HasForeignKey(k => k.ParentDocumentId);

                d.OwnsMany(d => d.Details).Property(e => e.LineTotalNet)
                                            .UsePropertyAccessMode(PropertyAccessMode.Property);

                d.OwnsMany(d => d.DocumentAddresses)
                 .WithOwner(a => a.Document)
                 .HasForeignKey(d => d.DocumentId);
            });            

            builder.Entity<Entity>(p =>
            {
                p.HasMany(e => e.RelatedProducts)
                 .WithMany(x => x.RelatedEntities);
                
                p.OwnsMany(a => a.EntityAdresses)
                 .WithOwner(e => e.Entity)
                 .HasForeignKey(e=>e.EntityId);
            });

            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
