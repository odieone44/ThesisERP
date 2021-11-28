using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThesisERP.Core.Entites;
using ThesisERP.Infrastracture.Configurations.Entities;

namespace ThesisERP.Infrastracture
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
            
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());          
        }
    }
}
