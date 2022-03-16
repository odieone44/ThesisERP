using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Stock;

public static class StockExtensions
{
    public async static Task<IEnumerable<StockLevel>> GetLocationStock(this IRepositoryBase<StockLevel> stockRepo, int? locationId)
    {
        var locationStock = await stockRepo
                            .GetAllAsync(
                                expression: x => (locationId == null || x.InventoryLocationId == locationId),
                                   orderBy: o => o.OrderBy(d => d.InventoryLocationId)
                                                    .ThenBy(x => x.ProductId),
                                   include: i => i.Include(p => p.Product)
                                                    .Include(l => l.InventoryLocation));

        return locationStock;
    }

    public async static Task<IEnumerable<StockLevel>> GetProductStock(this IRepositoryBase<StockLevel> stockRepo, int? productId)
    {
        var productStock = await stockRepo
                            .GetAllAsync(expression: x => (productId == null || x.ProductId == productId),
                                            orderBy: o => o.OrderBy(d => d.ProductId)
                                                            .ThenBy(x => x.InventoryLocationId),
                                            include: i => i.Include(p => p.Product)
                                                            .Include(l => l.InventoryLocation));

        return productStock;
    }

}
