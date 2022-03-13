using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Services.Stock;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Models;

namespace ThesisERP.Application.Services;


public class StockService : IStockService
{

    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IMapper _mapper;

    public StockService(IRepositoryBase<StockLevel> stockRepo, IMapper mapper)
    {
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    public async Task<List<GetLocationStockDTO>> GetLocationStock(int? locationId)
    {
        return await _stockRepo.GetLocationStock(locationId);
    }

    public async Task<List<GetProductStockDTO>> GetProductStock(int? productId)
    {
        return await _stockRepo.GetProductStock(productId);
    }

    public async Task HandleStockUpdateForTransactionRows(
        InventoryLocation location,
        IEnumerable<TransactionRowBase> transactionRows,
        TransactionStockAction stockAction)
    {
        if (!StockLevelUpdateHelper.GetActionsThatChangeStock().Contains(stockAction))
        {
            return;
        }       

        var stockDict = (await _stockRepo
                              .GetAllAsync(x => transactionRows.Select(x => x.Product.Id).Contains(x.ProductId)
                                           && x.InventoryLocationId == location.Id))
                              .ToDictionary(x => x.ProductId, v => v);

        var stockUpdateHelper = new StockLevelUpdateHelper(stockAction);
        
        foreach (var row in transactionRows)
        {
            if (!stockDict.TryGetValue(row.ProductId, out var locationStockEntry))
            {
                locationStockEntry = new StockLevel()
                {
                    InventoryLocation = location,
                    Product = row.Product
                };
                var result = _stockRepo.Add(locationStockEntry);
                stockDict.Add(row.ProductId, result);
            }

            stockUpdateHelper.HandleStockLevelUpdate(locationStockEntry, row.ProductQuantity);
            if (locationStockEntry.Id > 0) { _stockRepo.Update(locationStockEntry); }
        }
    }

    private class StockLevelUpdateHelper
    {        
        private readonly TransactionStockAction _stockAction;

        public StockLevelUpdateHelper(TransactionStockAction transStockAction)
        {            
            _stockAction = transStockAction;
        }

        public void HandleStockLevelUpdate(StockLevel stockEntry, decimal updateAmount)
        {
            if (GetActionsThatIncreaseIncomingStock().Contains(_stockAction))
            {
                stockEntry.Incoming += updateAmount;
            }
            else if (GetActionsThatDecreaseIncomingStock().Contains(_stockAction))
            {
                stockEntry.Incoming -= updateAmount;
            }

            if (GetActionsThatIncreaseOutgoingStock().Contains(_stockAction))
            {
                stockEntry.Outgoing += updateAmount;
            }
            else if (GetActionsThatDecreaseOutgoingStock().Contains(_stockAction))
            {
                stockEntry.Outgoing -= updateAmount;
            }

            if (GetActionsThatIncreaseAvailableStock().Contains(_stockAction))
            {
                stockEntry.Available += updateAmount;
            }
            else if (GetActionsThatDecreaseAvailableStock().Contains(_stockAction))
            {
                stockEntry.Available -= updateAmount;
                if (stockEntry.Available < 0)
                {
                    throw new ThesisERPException($"Cannot complete transaction as it will result in negative stock for product '{stockEntry.Product.SKU}' in location '{stockEntry.InventoryLocation.Name}'");
                }
            }
        }

        public static IEnumerable<TransactionStockAction> GetActionsThatChangeStock()
        {
            var actions = new List<TransactionStockAction>();
            
            actions.AddRange(GetActionsThatIncreaseIncomingStock());
            actions.AddRange(GetActionsThatDecreaseIncomingStock());
            actions.AddRange(GetActionsThatIncreaseOutgoingStock());
            actions.AddRange(GetActionsThatDecreaseOutgoingStock());
            actions.AddRange(GetActionsThatIncreaseAvailableStock());
            actions.AddRange(GetActionsThatDecreaseAvailableStock());

            return actions.Distinct();
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseIncomingStock()
        {
            yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.pending, StockChangeType.positive);
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseIncomingStock()
        {
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.positive);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.cancelled, StockChangeType.positive);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.draft, StockChangeType.positive);
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseOutgoingStock()
        {
            yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.pending, StockChangeType.negative);
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseOutgoingStock()
        {
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.negative);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.cancelled, StockChangeType.negative);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.draft, StockChangeType.negative);
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseAvailableStock()
        {
            yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.fulfilled, StockChangeType.positive);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.positive);
            yield return new TransactionStockAction(TransactionStatus.fulfilled, TransactionStatus.cancelled, StockChangeType.negative);
            yield return new TransactionStockAction(TransactionStatus.closed, TransactionStatus.cancelled, StockChangeType.negative);
        }

        private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseAvailableStock()
        {
            yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.fulfilled, StockChangeType.negative);
            yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.negative);
            yield return new TransactionStockAction(TransactionStatus.fulfilled, TransactionStatus.cancelled, StockChangeType.positive);
            yield return new TransactionStockAction(TransactionStatus.closed, TransactionStatus.cancelled, StockChangeType.positive);
        }

    }
}
