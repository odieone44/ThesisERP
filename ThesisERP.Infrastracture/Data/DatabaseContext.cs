using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Infrastracture.Data
{
    public class DatabaseContext : IdentityDbContext<AppUser>, IAppDbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Entity> Entities => Set<Entity>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Tax> Taxes => Set<Tax>();
        public DbSet<Discount> Discounts => Set<Discount>();
        public DbSet<Product> Products => Set<Product>();

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

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
