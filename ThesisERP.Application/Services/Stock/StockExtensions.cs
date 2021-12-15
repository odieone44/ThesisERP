using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Stock;

public static class StockExtensions
{
    public async static Task<List<GetLocationStockDTO>> GetLocationStock(this IRepositoryBase<StockLevel> stockRepo, int? locationId)
    {
        var stock = await stockRepo
                            .GetAllAsync(expression: x => (locationId == null || x.InventoryLocationId == locationId),
                                            orderBy: o => o.OrderBy(d => d.InventoryLocationId).ThenBy(x => x.ProductId),
                                            include: i => i.Include(p => p.Product).Include(l => l.InventoryLocation));

        var results = stock
                        .GroupBy(x => x.InventoryLocationId)
                        .Select(x => (id: x.Key, stockList: x.ToList()))
                        .Select(
                            x => new GetLocationStockDTO()
                            {
                                InventoryLocationId = x.id,
                                InventoryLocationName = x.stockList.FirstOrDefault().InventoryLocation.Name,
                                InventoryLocationAbbreviation = x.stockList.FirstOrDefault().InventoryLocation.Abbreviation,
                                ProductStockLevels = x.stockList.Select(s => new ProductStockLevelDTO()
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

    public async static Task<List<GetProductStockDTO>> GetProductStock(this IRepositoryBase<StockLevel> stockRepo, int? productId)
    {
        var stock = await stockRepo
                            .GetAllAsync(expression: x => (productId == null || x.ProductId == productId),
                                            orderBy: o => o.OrderBy(d => d.ProductId).ThenBy(x => x.InventoryLocationId),
                                            include: i => i.Include(p => p.Product).Include(l => l.InventoryLocation));

        var results = stock
                        .GroupBy(x => x.ProductId)
                        .Select(x => (id: x.Key, stockList: x.ToList()))
                        .Select(
                            x => new GetProductStockDTO()
                            {
                                ProductId = x.id,
                                ProductSKU = x.stockList.FirstOrDefault().Product.SKU,
                                ProductDescription = x.stockList.FirstOrDefault().Product.Description,
                                LocationStockLevels = x.stockList.Select(s => new LocationStockLevelDTO()
                                {
                                    InventoryLocationId = s.InventoryLocation.Id,
                                    InventoryLocationName = s.InventoryLocation.Name,
                                    InventoryLocationAbbreviation = s.InventoryLocation.Abbreviation!,
                                    Available = s.Available,
                                    Incoming = s.Incoming,
                                    Outgoing = s.Outgoing
                                }).ToList()
                            });

        return results.ToList();
    }

    //public async static Task UpdateTransactionProductsStockEntries<T>(this IRepositoryBase<StockLevel> stockRepo, T transaction, Core.Enums.Transactions.TransactionAction action) where T : ITransaction
    //{
    //    int locationId = transaction.InventoryLocation.Id;

    //    foreach (var detail in transaction.Details)
    //    {
    //        var locationStockEntry = detail.Product.StockLevels.FirstOrDefault(x=> x.InventoryLocationId == locationId);
    //        if (locationStockEntry is null)
    //        {
    //            locationStockEntry = new StockLevel()
    //            {
    //                InventoryLocationId = locationId,
    //                ProductId = detail.ProductId
    //            };
    //        }
    //    }
    //}

    //private void _HandleTransactionStockUpdate(StockLevel stockEntry, TransactionAction action, decimal quantity, bool isPositiveTransaction)
    //{
    //    switch (action)
    //    {
    //        case TransactionAction.create:
    //        case TransactionAction.update:
    //            if (isPositiveTransaction)
    //            {
    //                stockEntry.Incoming += quantity;
    //            } 
    //            else
    //            {
    //                stockEntry.Outgoing += quantity;
    //            } 
    //            break;
    //        case TransactionAction.fulfill:
    //            if (isPositiveTransaction)
    //            {
    //                stockEntry.Incoming -= quantity;
    //                stockEntry.Available += quantity;
    //            }
    //            else
    //            {
    //                stockEntry.Outgoing -= quantity;
    //                stockEntry.Available -= quantity;
    //            }
    //            break;
    //        case TransactionAction.cancel:
    //            if (isPositiveTransaction)
    //            {
    //                stockEntry.Incoming -= quantity;
    //                stockEntry.Available += quantity;
    //            }
    //            else
    //            {
    //                stockEntry.Outgoing -= quantity;
    //                stockEntry.Available -= quantity;
    //            }
    //            break;





    //    }
    //}
}
