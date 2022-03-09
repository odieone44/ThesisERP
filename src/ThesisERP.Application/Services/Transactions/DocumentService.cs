using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Models.Stock;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Extensions;

namespace ThesisERP.Application.Services.Transactions;

public class DocumentService : IDocumentService
{
    private readonly IApiService _api;
    private readonly IMapper _mapper;

    private Document _document;

    public DocumentService(IApiService apiService,
                           IMapper mapper)
    {
        _api = apiService;
        _mapper = mapper;
    }

    //TODO - This is a temp implementation for testing.
    public async Task<GenericDocumentDTO> Create(CreateDocumentDTO documentDTO, string username)
    {
        //todo - add transaction? 
        await _InitializeNewDocument(documentDTO, username);
        await _HandleStockUpdateForAction(TransactionAction.create);

        _document.Status = TransactionStatus.pending;
        _document.Comments = documentDTO.Comments;
        _document.Template.NextNumber++;

        var docResult = _api.DocumentsRepo.Add(_document);

        _api.DocumentTemplatesRepo.Update(_document.Template);

        await _api.DocumentsRepo.SaveChangesAsync();

        return _mapper.Map<GenericDocumentDTO>(docResult);
    }

    public async Task<GenericDocumentDTO> Update(int id, UpdateDocumentDTO documentDTO)
    {
        _document = await _api.DocumentsRepo.GetDocumentByIdIncludeRelations(id);
        _ = _document ?? throw new ThesisERPException($"Document with id: '{id}' not found.");

        switch (_document.Status)
        {
            case TransactionStatus.pending:
                //TODO - this is a temp way to handle stock. More elegant implementation pending.
                await _HandleStockUpdateForAction(TransactionAction.cancel);
                await _UpdatePendingDocumentWithNewValues(documentDTO);
                await _HandleStockUpdateForAction(TransactionAction.create);
                break;
            case TransactionStatus.fulfilled:
                break;
            default:
                throw new ThesisERPException($"Document with id: '{id}' cannot be updated because its status is '{_document.Status}'.");
        }

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        _document.BillingAddress = billAddress;
        _document.ShippingAddress = shipAddress;
        _document.Comments = documentDTO.Comments;
        _document.DateUpdated = DateTime.UtcNow;

        _api.DocumentsRepo.Update(_document);
        await _api.DocumentsRepo.SaveChangesAsync();
        
        return _mapper.Map<GenericDocumentDTO>(_document);        
    }

    public async Task<GenericDocumentDTO> Fulfill(int id)
    {
        _document = await _api.DocumentsRepo.GetDocumentByIdIncludeRelations(id);
        _ = _document ?? throw new ThesisERPException($"Document with id: '{id}' not found.");

        if (_document.Status != TransactionStatus.pending)
        {
            throw new ThesisERPException($"Document with id: '{id}' cannot be fulfilled because its status is not '{TransactionStatus.pending}'.");
        }

        await _HandleStockUpdateForAction(TransactionAction.fulfill);
        _document.Status = TransactionStatus.fulfilled;

        _api.DocumentsRepo.Update(_document);
        await _api.DocumentsRepo.SaveChangesAsync();

        return _mapper.Map<GenericDocumentDTO>(_document);
    }

    public async Task<GenericDocumentDTO> Close(int id)
    {
        _document = await _api.DocumentsRepo.GetDocumentByIdIncludeRelations(id);
        _ = _document ?? throw new ThesisERPException($"Document with id: '{id}' not found.");

        if (_document.Status != TransactionStatus.fulfilled)
        {
            throw new ThesisERPException($"Document with id: '{id}' cannot be closed because its status is not '{TransactionStatus.fulfilled}'.");
        }

        _document.Status = TransactionStatus.closed;
        _document.DateUpdated = DateTime.UtcNow;

        _api.DocumentsRepo.Update(_document);
        await _api.DocumentsRepo.SaveChangesAsync();

        return _mapper.Map<GenericDocumentDTO>(_document);
    }
    public async Task<GenericDocumentDTO> Cancel(int id)
    {
        _document = await _api.DocumentsRepo.GetDocumentByIdIncludeRelations(id);
        _ = _document ?? throw new ThesisERPException($"Document with id: '{id}' not found.");

        await _HandleStockUpdateForAction(TransactionAction.cancel);
        _document.Status = TransactionStatus.cancelled;
        _document.DateUpdated = DateTime.UtcNow;

        _api.DocumentsRepo.Update(_document);
        await _api.DocumentsRepo.SaveChangesAsync();

        return _mapper.Map<GenericDocumentDTO>(_document);        
    }

    private async Task _UpdatePendingDocumentWithNewValues(UpdateDocumentDTO documentDTO)
    {
        var productIds = documentDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document rows list has to be provided."); }

        var taxIds = documentDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = documentDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var entityId = documentDTO.EntityId ?? throw new ThesisERPException($"EntityId has to be provided when updating a pending document.");
        var locationId = documentDTO.InventoryLocationId ?? throw new ThesisERPException($"InventoryLocationId has to be provided when updating a pending document.");

        var newRequestValues = await _GetDocumentRequestValuesAsync(entityId, _document.TemplateId, locationId, productIds, taxIds, discountIds);

        _document.Entity = newRequestValues.Entity;
        _document.InventoryLocation = newRequestValues.InventoryLocation;

        var rowsList = new List<DocumentRow>();
        foreach (var (rowDTO, rowIndex) in documentDTO.Rows.WithIndex())
        {
            var thisProduct = newRequestValues.Products.FirstOrDefault(x => x.Id == rowDTO.ProductId);

            var thisTax = rowDTO.TaxID != null ? newRequestValues.Taxes.FirstOrDefault(x => x.Id == rowDTO.TaxID) : null;
            var thisDiscount = rowDTO.DiscountID != null ? newRequestValues.Discounts.FirstOrDefault(x => x.Id == rowDTO.DiscountID) : null;

            var detail = new DocumentRow(rowIndex, thisProduct, rowDTO.ProductQuantity, rowDTO.UnitPrice, thisTax, thisDiscount);
            rowsList.Add(detail);
        }

        _document.Rows = rowsList;
    }

    private async Task _InitializeNewDocument(CreateDocumentDTO documentDTO, string username)
    {
        var productIds = documentDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document rows list has to be provided."); }

        var taxIds = documentDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = documentDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var requestValues = await _GetDocumentRequestValuesAsync(documentDTO.EntityId, documentDTO.TemplateId, documentDTO.InventoryLocationId, productIds, taxIds, discountIds);

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        _document = Document.Initialize(requestValues.Entity,
                                        requestValues.InventoryLocation,
                                        requestValues.DocumentTemplate,
                                        billAddress,
                                        shipAddress,
                                        null,
                                        username);

        var rowsList = new List<DocumentRow>();
        foreach (var (rowDTO, rowIndex) in documentDTO.Rows.WithIndex())
        {
            var thisProduct = requestValues.Products.FirstOrDefault(x => x.Id == rowDTO.ProductId);

            var thisTax = rowDTO.TaxID != null ? requestValues.Taxes.FirstOrDefault(x => x.Id == rowDTO.TaxID) : null;
            var thisDiscount = rowDTO.DiscountID != null ? requestValues.Discounts.FirstOrDefault(x => x.Id == rowDTO.DiscountID) : null;

            var detail = new DocumentRow(rowIndex, thisProduct, rowDTO.ProductQuantity, rowDTO.UnitPrice, thisTax, thisDiscount);
            rowsList.Add(detail);
        }

        _document.Rows = rowsList;

    }

    private async Task _HandleStockUpdateForAction(TransactionAction action)
    {
        var stockAction = new TransactionStockAction(action, _document.Status, _document.Template.StockChangeType);
        int locationId = _document.InventoryLocation.Id;

        var stockDict = (await _api.StockRepo
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
                var result = _api.StockRepo.Add(locationStockEntry);
                stockDict.Add(row.ProductId, result);
            }

            stockUpdateHelper.HandleStockLevelUpdate(locationStockEntry);
            if (locationStockEntry.Id > 0) { _api.StockRepo.Update(locationStockEntry); }
        }
    }

    private async Task<DocumentRequestValues> _GetDocumentRequestValuesAsync(int entityId,
                                                                             int templateId,
                                                                             int locationId,
                                                                             List<int> productIds,
                                                                             List<int> taxIds,
                                                                             List<int> discountIds)
    {
        var template = await _api.DocumentTemplatesRepo.GetByIdAsync(templateId);
        _ = template ?? throw new ThesisERPException($"Document Template with id: '{templateId}' not found.");

        var entityType = template.UsesSupplierEntity ? EntityType.supplier : EntityType.client;
        var entity = (await _api.EntitiesRepo.GetAllAsync(x => x.Id == entityId && x.EntityType == entityType)).FirstOrDefault();
        _ = entity ?? throw new ThesisERPException($"{entityType} with id: '{entityId}' not found.");

        var location = await _api.LocationsRepo.GetByIdAsync(locationId);
        _ = location ?? throw new ThesisERPException($"Inventory Location with id: '{locationId}' not found.");

        if (!productIds.Any()) { throw new ThesisERPException("A valid document rows list has to be provided."); }

        var products = await _api.ProductsRepo.GetAllAsync(expression: x => productIds.Contains(x.Id),
                                                           include: i => i.Include(p => p.StockLevels));

        var nonExistingProducts = productIds.Except(products.Select(x => x.Id)).ToList();

        if (nonExistingProducts.Any())
        {
            throw new ThesisERPException($"Some products were not found: '{string.Join(", ", nonExistingProducts.Select(x => $"ProductId: {x}"))}'. Document Creation failed.");
        }

        var taxes = new List<Tax>();

        if (taxIds.Any())
        {
            taxes = await _api.TaxesRepo.GetAllAsync(expression: x => taxIds.Contains(x.Id));
            var nonExistingTaxes = taxIds.Except(taxes.Select(x => x.Id)).ToList();

            if (nonExistingTaxes.Any())
            {
                throw new ThesisERPException($"Some taxes were not found: '{string.Join(", ", nonExistingProducts.Select(x => $"TaxId: {x}"))}'. Document Creation failed.");
            }
        }

        var discounts = new List<Discount>();

        if (discountIds.Any())
        {
            discounts = await _api.DiscountsRepo.GetAllAsync(expression: x => discountIds.Contains(x.Id));
            var nonExistingDiscounts = discountIds.Except(discounts.Select(x => x.Id)).ToList();

            if (nonExistingDiscounts.Any())
            {
                throw new ThesisERPException($"Some discounts were not found: '{string.Join(", ", nonExistingProducts.Select(x => $"DiscountID: {x}"))}'. Document Creation failed.");
            }
        }

        return new DocumentRequestValues(entity, location, template, products, taxes, discounts);
    }

    private class DocumentRequestValues
    {
        public Entity Entity { get; set; }
        public InventoryLocation InventoryLocation { get; set; }
        public DocumentTemplate DocumentTemplate { get; set; }
        public List<Product> Products { get; set; }
        public List<Tax> Taxes { get; set; }
        public List<Discount> Discounts { get; set; }

        public DocumentRequestValues(Entity entity, InventoryLocation location, DocumentTemplate template, List<Product> products, List<Tax> taxes, List<Discount> discounts)
        {
            Entity = entity;
            InventoryLocation = location;
            DocumentTemplate = template;
            Products = products;
            Taxes = taxes;
            Discounts = discounts;
        }
    }

}
