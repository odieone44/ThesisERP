using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Stock;

public static class StockServices
{
    public async static Task<List<LocationStockDTO>> GetLocationStock(this IRepositoryBase<StockLevel> stockRepo, int? locationId)
    {

        Expression<Func<StockLevel, bool>> queryExp = x => (locationId == null || x.InventoryLocationId == locationId);

        var stock = await stockRepo
                            .GetAllAsync(
                            expression: queryExp,
                               orderBy: o => o.OrderBy(d => d.InventoryLocationId).ThenBy(x => x.ProductId),
                               include: x => x.Include(p => p.Product).Include(l => l.InventoryLocation));
        
        var results = stock
                        .GroupBy(x => x.InventoryLocationId)
                        .Select(x => new LocationStockDTO()
                                {
                                    InventoryLocationId = x.Key,
                                    InventoryLocationName = x.Select(l => l.InventoryLocation.Name).FirstOrDefault()!,
                                    InventoryLocationAbbreviation = x.Select(l => l.InventoryLocation.Abbreviation).FirstOrDefault()!,
                                    ProductStockLevels = x.Select(s => new ProductStockLevelDTO()
                                    {
                                        ProductSKU = s.Product.SKU!,
                                        ProductId = s.ProductId,
                                        ProductDescription = s.Product.Description!,
                                        Available = s.Available,
                                        Incoming = s.Incoming,
                                        Outgoing = s.Outgoing
                                    }).ToList()
                                });

        return results.ToList();
    }
}
