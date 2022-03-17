using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Extensions;
using ThesisERP.Core.Models;

namespace ThesisERP.Application.Services.Transactions;

public class DocumentService : IDocumentService
{
    private readonly IApiService _api;
    private readonly IStockService _stockService;
    private readonly IMapper _mapper;

    private Document _document;

    public DocumentService(IApiService apiService,
                           IStockService stockService,
                           IMapper mapper)
    {
        _api = apiService;
        _stockService = stockService;
        _mapper = mapper;
    }

    public async Task<GenericDocumentDTO> Create(
        CreateDocumentDTO documentDTO, 
        string username, 
        Order? parentOrder = null)
    {
        await _InitializeNewDocument(documentDTO, username, parentOrder);
        _document.Status = documentDTO.CreateAsFulfilled ? TransactionStatus.fulfilled : TransactionStatus.pending;

        var stockAction = new TransactionStockAction(
            TransactionStatus.draft,
            _document.Status,
            _document.Template.StockChangeType);

        await _stockService.HandleStockUpdateFromDocumentAction(_document, stockAction);

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

        if (!_document.CanBeUpdated)
        {
            throw new ThesisERPException($"Document with id: '{id}' cannot be updated because its status is '{_document.Status}'.");
        }

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        _document.BillingAddress = billAddress;
        _document.ShippingAddress = shipAddress;
        _document.Comments = documentDTO.Comments;
        _document.DateUpdated = DateTime.UtcNow;

        //only allow document rows and location to be updated if document is pending/draft.
        if (_document.Status == TransactionStatus.pending ||
             _document.Status == TransactionStatus.draft)
        {
            var stockActionForOldRows = new TransactionStockAction(
                _document.Status,
                TransactionStatus.draft,
                _document.Template.StockChangeType);

            await _stockService.HandleStockUpdateFromDocumentAction(_document, stockActionForOldRows);

            await _UpdatePendingDocumentWithNewValues(documentDTO);

            var stockActionForNewRows = 
                new TransactionStockAction(
                  TransactionStatus.draft,
                  _document.Status,
                  _document.Template.StockChangeType);

            await _stockService.HandleStockUpdateFromDocumentAction(_document, stockActionForNewRows);
        }

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

        var stockAction = new TransactionStockAction(
               _document.Status,
               TransactionStatus.fulfilled,
               _document.Template.StockChangeType);

        await _stockService.HandleStockUpdateFromDocumentAction(_document, stockAction);

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

        var stockAction = new TransactionStockAction(
               _document.Status,
               TransactionStatus.cancelled,
               _document.Template.StockChangeType);

        await _stockService.HandleStockUpdateFromDocumentAction(_document, stockAction);

        _document.Status = TransactionStatus.cancelled;
        _document.DateUpdated = DateTime.UtcNow;

        _api.DocumentsRepo.Update(_document);
        await _api.DocumentsRepo.SaveChangesAsync();

        return _mapper.Map<GenericDocumentDTO>(_document);
    }

    private async Task _UpdatePendingDocumentWithNewValues(UpdateDocumentDTO documentDTO)
    {
        var productIds = documentDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document row list has to be provided."); }

        var taxIds = documentDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = documentDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var entityId = documentDTO.EntityId ?? throw new ThesisERPException($"EntityId has to be provided when updating a pending document.");
        var locationId = documentDTO.InventoryLocationId ?? throw new ThesisERPException($"InventoryLocationId has to be provided when updating a pending document.");

        var newRequestValues = await _GetDocumentRequestValuesAsync(entityId, _document.TemplateId, locationId, productIds, taxIds, discountIds);

        _document.Entity = newRequestValues.Entity;
        _document.InventoryLocation = newRequestValues.InventoryLocation;

        _AssignRowsToDocument(documentDTO.Rows, newRequestValues);
    }

    private async Task _InitializeNewDocument(CreateDocumentDTO documentDTO, string username, Order? parentOrder = null)
    {
        var productIds = documentDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document rows list has to be provided."); }

        var taxIds = documentDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = documentDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var requestValues = await _GetDocumentRequestValuesAsync(documentDTO.EntityId, documentDTO.TemplateId, documentDTO.InventoryLocationId, productIds, taxIds, discountIds);

        if (parentOrder is not null && 
            !parentOrder.CanBeFulfilledBy(requestValues.DocumentTemplate.DocumentType))
        {
            throw new ThesisERPException($"Orders of type '{parentOrder.Type}' cannot be fulfilled by documents of type '{requestValues.DocumentTemplate.DocumentType}'. Document creation failed.");
        }

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        _document = Document.Initialize(requestValues.Entity,
                                        requestValues.InventoryLocation,
                                        requestValues.DocumentTemplate,
                                        billAddress,
                                        shipAddress,
                                        parentOrder,
                                        username);

        _AssignRowsToDocument(documentDTO.Rows, requestValues);

    }

    private void _AssignRowsToDocument(IEnumerable<CreateDocumentRowDTO> documentDtoRows, DocumentRequestValues requestValues)
    {
        var rowsList = new List<DocumentRow>();
        foreach (var (rowDTO, rowIndex) in documentDtoRows.WithIndex())
        {
            var thisProduct = requestValues.Products.FirstOrDefault(x => x.Id == rowDTO.ProductId);

            var thisTax = rowDTO.TaxID != null ? requestValues.Taxes.FirstOrDefault(x => x.Id == rowDTO.TaxID) : null;
            var thisDiscount = rowDTO.DiscountID != null ? requestValues.Discounts.FirstOrDefault(x => x.Id == rowDTO.DiscountID) : null;

            var detail = new DocumentRow(rowIndex + 1, thisProduct, rowDTO.ProductQuantity, rowDTO.UnitPrice, thisTax, thisDiscount);
            rowsList.Add(detail);
        }

        _document.Rows = rowsList;
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
            throw new ThesisERPException($"Some products were not found: '{string.Join(", ", nonExistingProducts)}'. Document Creation failed.");
        }

        var taxes = new List<Tax>();

        if (taxIds.Any())
        {
            taxes = await _api.TaxesRepo.GetAllAsync(expression: x => taxIds.Contains(x.Id));
            var nonExistingTaxes = taxIds.Except(taxes.Select(x => x.Id)).ToList();

            if (nonExistingTaxes.Any())
            {
                throw new ThesisERPException($"Some taxes were not found: '{string.Join(", ", nonExistingTaxes)}'. Document Creation failed.");
            }
        }

        var discounts = new List<Discount>();

        if (discountIds.Any())
        {
            discounts = await _api.DiscountsRepo.GetAllAsync(expression: x => discountIds.Contains(x.Id));
            var nonExistingDiscounts = discountIds.Except(discounts.Select(x => x.Id)).ToList();

            if (nonExistingDiscounts.Any())
            {
                throw new ThesisERPException($"Some discounts were not found: '{string.Join(", ",nonExistingDiscounts)}'. Document Creation failed.");
            }
        }

        return new DocumentRequestValues(entity, location, template, products, taxes, discounts);
    }

    public async Task<List<GenericDocumentDTO>> GetDocuments()
    {
        var documents = await _api.DocumentsRepo
                               .GetAllAsync
                                (orderBy: o => o.OrderByDescending(d => d.DateUpdated),
                                include: i => i.Include(p => p.Entity)
                                               .Include(x => x.InventoryLocation)
                                               .Include(t => t.Template)
                                               .Include(p => p.ParentOrder)
                                               .Include(q => q.Rows)
                                                   .ThenInclude(d => d.Product)
                                               .Include(q => q.Rows)
                                                   .ThenInclude(d => d.Tax)
                                               .Include(q => q.Rows)
                                                   .ThenInclude(d => d.Discount));

        var results = _mapper.Map<List<GenericDocumentDTO>>(documents);

        return results;
    }

    public async Task<GenericDocumentDTO?> GetDocument(int id)
    {
        var document = await _api.DocumentsRepo.GetDocumentByIdIncludeRelations(id);

        if (document == null) { return null; }

        var result = _mapper.Map<GenericDocumentDTO>(document);

        return result;
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
