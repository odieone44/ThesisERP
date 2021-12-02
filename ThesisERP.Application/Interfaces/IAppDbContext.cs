using Microsoft.EntityFrameworkCore;
using ThesisERP.Core.Entites;

namespace ThesisERP.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<AppUser> AppUsers { get; }
        DbSet<Entity> Entities { get; }
        DbSet<Document> Documents { get; }
        DbSet<Tax> Taxes { get; }
        DbSet<Discount> Discounts { get; }
        DbSet<Product> Products { get; }
        Task<int> SaveChangesAsync();
    }
}
