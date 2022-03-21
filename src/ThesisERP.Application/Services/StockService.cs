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
        var stock =  await _stockRepo.GetLocationStock(locationId);

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

    public async Task<List<GetProductStockDTO>> GetProductStock(int? productId)
    {
        var stock = await _stockRepo.GetProductStock(productId);

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

    public async Task HandleStockUpdateFromDocumentAction(Document document, TransactionStockAction stockAction)
    {
        _transactionRows = document.Rows;
        _location = document.InventoryLocation;
        _action = stockAction;

        await _HandleStockUpdate();
    }

    private async Task _HandleStockUpdate()
    {
        if (!_action.HasStockEffect)
        {
            return;
        }

        var stockList = await _stockRepo
                             .GetAllAsync(x => _transactionRows.Select(x => x.Product.Id).Contains(x.ProductId)
                                          && x.InventoryLocationId == _location.Id);
        
        var stockDict = stockList.ToDictionary(x => x.ProductId, v => v);

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

            _UpdateStockLevelProperties(locationStockEntry, row.ProductQuantity);
            if (locationStockEntry.Id > 0) { _stockRepo.Update(locationStockEntry); }
        }

    }

    private void _UpdateStockLevelProperties(StockLevel stockEntry, decimal updateAmount)
    {
        if (_action.IncreasesIncomingStock)
        {
            stockEntry.Incoming += updateAmount;
        }
        else if (_action.DecreasesIncomingStock)
        {
            stockEntry.Incoming -= updateAmount;
        }

        if (_action.IncreasesOutgoingStock)
        {
            stockEntry.Outgoing += updateAmount;
        }
        else if (_action.DecreasesOutgoingStock)
        {
            stockEntry.Outgoing -= updateAmount;
        }

        if (_action.IncreasesAvailableStock)
        {
            stockEntry.Available += updateAmount;
        }
        else if (_action.DecreasesAvailableStock)
        {
            stockEntry.Available -= updateAmount;
            if (stockEntry.Available < 0)
            {
                throw new ThesisERPException($"Cannot complete transaction as it will result in negative stock for product '{stockEntry.Product.SKU}' in location '{stockEntry.InventoryLocation.Name}'");
            }
        }
    }

}
