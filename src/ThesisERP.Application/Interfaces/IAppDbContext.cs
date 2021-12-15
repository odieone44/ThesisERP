using Microsoft.EntityFrameworkCore;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<AppUser> AppUsers { get; }
    DbSet<Entity> Entities { get; }
    DbSet<Document> Documents { get; }
    DbSet<DocumentTemplate> DocumentTemplates { get; }
    DbSet<InventoryLocation> InventoryLocations { get; }
    DbSet<StockLevel> StockLevels { get; }
    DbSet<Tax> Taxes { get; }
    DbSet<Discount> Discounts { get; }
    DbSet<Product> Products { get; }
    Task<int> SaveChangesAsync();
}
