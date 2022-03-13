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

    private IEnumerable<TransactionRowBase> _transactionRows;
    private InventoryLocation _location;
    private TransactionStockAction _action;

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

    public async Task HandleStockUpdateFromDocumentAction(Document document, TransactionStockAction stockAction)
    {
        _transactionRows = document.Rows;
        _location = document.InventoryLocation;
        _action = stockAction;

        await _HandleStockUpdate();
    }

    private async Task _HandleStockUpdate()
    {
        //if (!StockLevelUpdateHelper.GetActionsThatChangeStock().Contains(_action))
        //{
        //    return;
        //}

        var stockDict = (await _stockRepo
                             .GetAllAsync(x => _transactionRows.Select(x => x.Product.Id).Contains(x.ProductId)
                                          && x.InventoryLocationId == _location.Id))
                             .ToDictionary(x => x.ProductId, v => v);

        var stockUpdateHelper = new StockLevelUpdateHelper(_action);

        foreach (var row in _transactionRows)
        {
            if (row.Product.Type == ProductType.service) { continue; }

            if (!stockDict.TryGetValue(row.ProductId, out var locationStockEntry))
            {
                locationStockEntry = new StockLevel()
                {
                    InventoryLocation = _location,
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
            if (_stockAction.IncreasesIncomingStock)
            {
                stockEntry.Incoming += updateAmount;
            }
            else if (_stockAction.DecreasesIncomingStock)
            {
                stockEntry.Incoming -= updateAmount;
            }

            if (_stockAction.IncreasesOutgoingStock)
            {
                stockEntry.Outgoing += updateAmount;
            }
            else if (_stockAction.DecreasesOutgoingStock)
            {
                stockEntry.Outgoing -= updateAmount;
            }

            if (_stockAction.IncreasesAvailableStock)
            {
                stockEntry.Available += updateAmount;
            }
            else if (_stockAction.DecreasesAvailableStock)
            {
                stockEntry.Available -= updateAmount;
                if (stockEntry.Available < 0)
                {
                    throw new ThesisERPException($"Cannot complete transaction as it will result in negative stock for product '{stockEntry.Product.SKU}' in location '{stockEntry.InventoryLocation.Name}'");
                }
            }
        }

    }
}
