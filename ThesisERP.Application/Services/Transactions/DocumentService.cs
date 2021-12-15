using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Models.Stock;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Enums;


namespace ThesisERP.Application.Services.Transactions;

public class DocumentService : IDocumentService
{
    private readonly IRepositoryBase<Document> _documentsRepo;
    private readonly IRepositoryBase<Product> _productsRepo;
    private readonly IRepositoryBase<DocumentTemplate> _templatesRepo;
    private readonly IRepositoryBase<Entity> _entitiesRepo;
    private readonly IRepositoryBase<InventoryLocation> _locationsRepo;
    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IMapper _mapper;    

    private Document _document;

    public DocumentService(IRepositoryBase<Document> documentsRepo,
                           IRepositoryBase<Product> productsRepo,
                           IRepositoryBase<DocumentTemplate> templatesRepo,
                           IRepositoryBase<Entity> entitiesRepo,
                           IRepositoryBase<InventoryLocation> locationsRepo,
                           IRepositoryBase<StockLevel> stockRepo,
                           IMapper mapper)
    {
        _documentsRepo = documentsRepo;
        _productsRepo = productsRepo;
        _templatesRepo = templatesRepo;
        _entitiesRepo = entitiesRepo;
        _locationsRepo = locationsRepo;
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    public Task<Document> Cancel(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Document> Close(int id)
    {
        throw new NotImplementedException();
    }

    //TODO - This is a temp implementation for testing.
    public async Task<Document> Create(CreateDocumentDTO documentDTO, string username)
    {
        //todo - add transaction? 
        await _InitializeNewDocument(documentDTO, username);
        await _HandleStockUpdateForAction(TransactionAction.create);

        _document.Status = TransactionStatus.pending;
        _document.Comments = documentDTO.Comments;        
        _document.DocumentTemplate.NextNumber++;

        var docResult = _documentsRepo.Add(_document);

        _templatesRepo.Update(_document.DocumentTemplate);

        await _documentsRepo.SaveChangesAsync();

        return docResult;
    }

    public async Task<Document> Fulfill(int id)
    {
        _document = await _documentsRepo.GetDocumentByIdIncludeRelations(id);
        _ = _document ?? throw new ThesisERPException($"Document with id: '{id}' not found.");

        if (_document.Status != TransactionStatus.pending)
        {
            throw new ThesisERPException($"Document with id: '{id}' cannot be fulfilled because its not marked as 'Pending'.");
        }

        await _HandleStockUpdateForAction(TransactionAction.fulfill);
        _document.Status = TransactionStatus.fulfilled;

        _documentsRepo.Update(_document);

        await _documentsRepo.SaveChangesAsync();

        return _document;
    }

    public Task<Document> Update(int id, UpdateDocumentDTO documentDTO)
    {
        throw new NotImplementedException();
    }

    private async Task _InitializeNewDocument(CreateDocumentDTO documentDTO, string username)
    {
        var template = await _templatesRepo.GetByIdAsync(documentDTO.TemplateId);
        _ = template ?? throw new ThesisERPException($"Document Template with id: '{documentDTO.TemplateId}' not found.");

        var entityType = template.IsPositiveStockTransaction ? EntityType.supplier : EntityType.client;
        var entity = (await _entitiesRepo.GetAllAsync(x => x.Id == documentDTO.EntityId && x.EntityType == entityType)).FirstOrDefault();
        _ = entity ?? throw new ThesisERPException($"{entityType} with id: '{documentDTO.EntityId}' not found.");

        var location = await _locationsRepo.GetByIdAsync(documentDTO.InventoryLocationId);
        _ = location ?? throw new ThesisERPException($"Inventory Location with id: '{documentDTO.InventoryLocationId}' not found.");

        var productIds = documentDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document rows list has to be provided."); }

        var products = await _productsRepo.GetAllAsync(expression: x => productIds.Contains(x.Id),
                                                       include: i => i.Include(p => p.StockLevels));

        var nonExistingProducts = productIds.Except(products.Select(x => x.Id)).ToList();

        if (nonExistingProducts.Any())
        {
            throw new ThesisERPException($"Some products were not found: '{string.Join(", ", nonExistingProducts.Select(x => $"ProductId: {x}"))}'. Document Creation failed.");
        }

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        _document = Document.Initialize(entity, location, template, billAddress, shipAddress, username);

        var rowsList = new List<DocumentRow>();
        foreach (var rowDTO in documentDTO.Rows)
        {
            var thisProduct = products.Where(x => x.Id == rowDTO.ProductId).FirstOrDefault();

            //todo add taxes/discounts
            var detail = new DocumentRow(thisProduct, rowDTO.ProductQuantity, rowDTO.UnitPrice, null, null);
            rowsList.Add(detail);
        }

        _document.Rows = rowsList;

    }

    private async Task _HandleStockUpdateForAction(TransactionAction action)
    {      

        var stockAction = new TransactionStockAction(action, _document.Status, _document.Type);
        int locationId = _document.InventoryLocation.Id;

        var stockDict = (await _stockRepo
                              .GetAllAsync(x => _document.Rows.Select(x => x.Product.Id).Contains(x.ProductId)
                                           && x.InventoryLocationId == locationId))
                              .ToDictionary(x => x.ProductId, v => v);

        foreach (var row in _document.Rows)
        {
            var stockUpdateHelper = new StockLevelUpdateHelper(stockAction, row.ProductQuantity);           

            if (!stockDict.TryGetValue(row.ProductId, out var locationStockEntry))
            {
                locationStockEntry = new StockLevel()
                {
                    InventoryLocation = _document.InventoryLocation,
                    Product = row.Product
                };
                var result = _stockRepo.Add(locationStockEntry);
                stockDict.Add(row.ProductId, result);                
            }
            stockUpdateHelper.HandleStockLevelUpdate(locationStockEntry);
            if (locationStockEntry.Id > 0) { _stockRepo.Update(locationStockEntry); }
        }
    }

}
