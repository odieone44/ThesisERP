using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Services.Entities;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Enums;
using static ThesisERP.Core.Enums.Transactions;
using static ThesisERP.Core.Enums.Entities;

namespace ThesisERP.Application.Services.Transactions;

public class DocumentService : IDocumentService
{
    private readonly IRepositoryBase<Document> _documentsRepo;
    private readonly IRepositoryBase<Product> _productsRepo;
    private readonly IRepositoryBase<TransactionTemplate> _templatesRepo;
    private readonly IRepositoryBase<Entity> _entitiesRepo;
    private readonly IRepositoryBase<InventoryLocation> _locationsRepo;
    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IMapper _mapper;
    
    private TransactionTemplate _template;
    private Entity _entity;
    private InventoryLocation _location;
    private List<Product> _products;

    public DocumentService(IRepositoryBase<Document> documentsRepo,
                           IRepositoryBase<Product> productsRepo,
                           IRepositoryBase<TransactionTemplate> templatesRepo,
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
        await _LoadDocumentCreateRequiredEntities(documentDTO);

        var billAddress = _mapper.Map<Address>(documentDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(documentDTO.ShippingAddress);

        var detailsList = new List<DocumentDetail>();
        
        var stockDict = (await _stockRepo
                               .GetAllAsync(x => _products.Select(x=>x.Id).Contains(x.ProductId) 
                                            && x.InventoryLocationId == documentDTO.InventoryLocationId))
                               .ToDictionary(x => x.ProductId, v => v);

        foreach (var detailDto in documentDTO.Details)
        {
            var thisProduct = _products.Where(x => x.Id == detailDto.ProductId).FirstOrDefault();
            
            //todo add taxes/discounts
            var detail = new DocumentDetail(thisProduct, detailDto.ProductQuantity, detailDto.UnitPrice, null, null);
            detailsList.Add(detail);

            if (stockDict.TryGetValue(detailDto.ProductId, out var stockEntry))
            {
                if (_template.IsPositiveStockTransaction)
                {
                    stockEntry.Incoming += detail.ProductQuantity;
                }
                else
                {
                    stockEntry.Outgoing += detail.ProductQuantity;
                }

                await _stockRepo.UpdateAsync(stockEntry);
            }
            else
            {
                var newStock = new StockLevel()
                {
                    InventoryLocation = _location,
                    Product = thisProduct,
                    Available = decimal.Zero,
                    Incoming = _template.IsPositiveStockTransaction ? detail.ProductQuantity : decimal.Zero,
                    Outgoing = _template.IsPositiveStockTransaction ? decimal.Zero : detail.ProductQuantity
                };

                var result = await _stockRepo.AddAsync(newStock);

                stockDict.Add(detailDto.ProductId, result);
            }

        }

        var doc = new Document(_entity, 
                               _location, 
                               _template, 
                               billAddress, 
                               shipAddress, 
                               detailsList, 
                               documentDTO.Comments, 
                               username);

        _template.NextNumber++;

        var docResult = await _documentsRepo.AddAsync(doc);
        
        await _templatesRepo.UpdateAsync(_template);
        
        return docResult;
    }

    public Task<Document> Fulfill(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Document> Update(int id, UpdateDocumentDTO documentDTO)
    {
        throw new NotImplementedException();
    }

    private async Task _LoadDocumentCreateRequiredEntities(CreateDocumentDTO documentDTO)
    {
        _template = await _templatesRepo.GetByIdAsync(documentDTO.TemplateId);
        _ = _template ?? throw new ThesisERPException($"Document Template with id: '{documentDTO.TemplateId}' not found.");

        var entityType = _template.IsPositiveStockTransaction ? EntityTypes.supplier : EntityTypes.client;
        _entity = (await _entitiesRepo.GetAllAsync(x => x.Id == documentDTO.EntityId && x.EntityType == entityType)).FirstOrDefault();
        _ = _entity ?? throw new ThesisERPException($"{entityType} with id: '{documentDTO.EntityId}' not found.");

        _location = await _locationsRepo.GetByIdAsync(documentDTO.InventoryLocationId);
        _ = _location ?? throw new ThesisERPException($"Inventory Location with id: '{documentDTO.InventoryLocationId}' not found.");

        var productIds = documentDTO.Details.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid document details list has to be provided."); }

        _products = await _productsRepo.GetAllAsync(expression: x => productIds.Contains(x.Id),
                                                       include: i => i.Include(p => p.StockLevels));

        var nonexistingProducts = productIds.Except(_products.Select(x => x.Id)).ToList();

        if (nonexistingProducts.Any())
        {
            throw new ThesisERPException($"Some products were not found: '{string.Join(", ", nonexistingProducts)}'. Document Creation failed.");
        }
    }
    
}
