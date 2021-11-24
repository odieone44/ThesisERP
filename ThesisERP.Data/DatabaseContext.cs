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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //builder.Entity<Address>().HasKey(x => new { x.EntityType, x.EntityId, x.AddressType});
            //builder.Entity<AppUser>().OwnsMany(t => t.RefreshTokens);
            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
