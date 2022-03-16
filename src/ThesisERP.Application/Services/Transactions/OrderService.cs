using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Extensions;

namespace ThesisERP.Application.Services.Transactions;

public class OrderService : IOrderService
{
    private readonly IApiService _api;
    private readonly IDocumentService _documentService;
    private readonly IMapper _mapper;

    private Order _order;

    public OrderService(IApiService apiService,
                        IDocumentService documentService,
                        IMapper mapper)
    {
        _api = apiService;
        _documentService = documentService;
        _mapper = mapper;
    }

    public async Task<GenericOrderDTO> Create(CreateOrderDTO createOrderDTO, string username)
    {
        await _InitializeNewOrder(createOrderDTO, username);

        _order.Status = TransactionStatus.pending;
        _order.Comments = createOrderDTO.Comments;
        _order.Template.NextNumber++;

        var orderResult = _api.OrdersRepo.Add(_order);

        _api.OrderTemplatesRepo.Update(_order.Template);

        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(orderResult);
    }

    public async Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateOrderDTO)
    {
        _order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);
        _ = _order ?? throw new ThesisERPException($"Order with id: '{id}' not found.");

        if (!_order.CanBeUpdated)
        {
            throw new ThesisERPException($"Order with id: '{id}' cannot be updated because its status is '{_order.Status}'.");
        }

        var billAddress = _mapper.Map<Address>(updateOrderDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(updateOrderDTO.ShippingAddress);

        _order.BillingAddress = billAddress;
        _order.ShippingAddress = shipAddress;
        _order.Comments = updateOrderDTO.Comments;
        _order.DateUpdated = DateTime.UtcNow;

        if (_order.Status == TransactionStatus.pending ||
             _order.Status == TransactionStatus.draft)
        {
            await _UpdatePendingOrderWithNewValues(updateOrderDTO);
        }

        _api.OrdersRepo.Update(_order);
        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(_order);
    }

    public async Task<GenericOrderDTO> Process(int id, ProcessOrderDTO processOrderDTO)
    {
        _order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);
        _ = _order ?? throw new ThesisERPException($"Order with id: '{id}' not found.");

        if (!(_order.Status == TransactionStatus.pending || _order.Status == TransactionStatus.draft))
        {
            throw new ThesisERPException($"Order with id: '{id}' cannot be processed because its status is not '{TransactionStatus.pending}' or '{TransactionStatus.draft}'.");
        }

        var location = await _api.LocationsRepo.GetByIdAsync(processOrderDTO.InventoryLocationId);
        _ = location ?? throw new ThesisERPException($"Failed to process order: Location with id '{processOrderDTO.InventoryLocationId}' does not exist or is deleted.");

        _order.InventoryLocation = location;
        _order.Status = TransactionStatus.processing;

        _api.OrdersRepo.Update(_order);
        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(_order);
    }

    public async Task<GenericOrderDTO> Fulfill(int id, FulfillOrderDTO fulfillOrderDTO)
    {
        _order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);
        _ = _order ?? throw new ThesisERPException($"Order with id: '{id}' not found.");

        if (_order.Status != TransactionStatus.processing)
        {
            throw new ThesisERPException($"Order with id: '{id}' cannot be fulfilled because its status is not '{TransactionStatus.processing}'.");
        }

        _ValidateFulfillmentRows(fulfillOrderDTO);

        Dictionary<int, FulfillOrderRowDTO> rowsToFulfill;
        var fulfillAllRows = fulfillOrderDTO.FullfillmentRows.IsNullOrEmpty();

        if (fulfillAllRows)
        {
            rowsToFulfill = _order.Rows
                                .Where(x => x.RemainingQuantity > 0)
                                .ToDictionary(
                                    key => key.LineNumber,
                                    val => new FulfillOrderRowDTO()
                                    {
                                        LineNumber = val.LineNumber,
                                        QuantityToFulfill = val.ProductQuantity
                                    });
        }
        else
        {
            rowsToFulfill = fulfillOrderDTO.FullfillmentRows
                            .ToDictionary(key => key.LineNumber);
        }

        var newDocumentRows = new List<CreateDocumentRowDTO>();
        foreach (var row in _order.Rows)
        {
            if (!rowsToFulfill.TryGetValue(row.LineNumber, out var fulfillmentRow))
            {
                continue;
            }

            row.FulfilledQuantity += fulfillmentRow.QuantityToFulfill;

            var newRow = new CreateDocumentRowDTO()
            {
                ProductId = row.ProductId,
                ProductQuantity = fulfillmentRow.QuantityToFulfill,
                DiscountID = row.DiscountID,
                TaxID = row.TaxID,
                UnitPrice = row.UnitPrice
            };

            newDocumentRows.Add(newRow);
        }

        var documentDto = new CreateDocumentDTO()
        {
            BillingAddress = _mapper.Map<AddressDTO>(_order.BillingAddress.Copy()),
            ShippingAddress = _mapper.Map<AddressDTO>(_order.ShippingAddress.Copy()),
            Comments = _order.Comments,
            EntityId = _order.EntityId,
            InventoryLocationId = _order.InventoryLocationId.GetValueOrDefault(),
            CreateAsFulfilled = true, //!fulfillOrderDTO.CreateAsPending //todo: rethink this,
            TemplateId = fulfillOrderDTO.FulfillmentDocumentTemplateId,
            Rows = newDocumentRows
        };

        if (_order.Rows.All(r => r.RowIsFulfilled))
        {
            _order.Status = TransactionStatus.fulfilled;
        }

        _api.OrdersRepo.Update(_order);

        var createdDocument = _documentService.Create(documentDto, _order.CreatedBy, _order);

        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(_order);
    }

    public async Task<GenericOrderDTO> Close(int id)
    {
        _order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);
        _ = _order ?? throw new ThesisERPException($"Order with id: '{id}' not found.");

        if (_order.Status != TransactionStatus.fulfilled)
        {
            throw new ThesisERPException($"Order with id: '{id}' cannot be closed because its status is not '{TransactionStatus.fulfilled}'.");
        }

        _order.Status = TransactionStatus.closed;
        _order.DateUpdated = DateTime.UtcNow;

        _api.OrdersRepo.Update(_order);
        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(_order);
    }

    public async Task<GenericOrderDTO> Cancel(int id)
    {
        _order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);
        _ = _order ?? throw new ThesisERPException($"Order with id: '{id}' not found.");

        if (_order.RelatedDocuments.Any(r => r.Status != TransactionStatus.cancelled))
        {
            throw new ThesisERPException($"Order with id: '{id}' cannot be cancelled because it has open related documents. Try cancelling them first.");
        }

        _order.Status = TransactionStatus.cancelled;
        _order.DateUpdated = DateTime.UtcNow;

        _api.OrdersRepo.Update(_order);
        await _api.OrdersRepo.SaveChangesAsync();

        return _mapper.Map<GenericOrderDTO>(_order);
    }

    private async Task _InitializeNewOrder(CreateOrderDTO orderDTO, string username)
    {
        var productIds = orderDTO.Rows
                            .Select(x => x.ProductId)
                            .Distinct()
                            .ToList();

        if (!productIds.Any()) { throw new ThesisERPException("A valid order rows list has to be provided."); }

        var taxIds = orderDTO.Rows
                        .Where(y => y.TaxID != null)
                        .Select(x => (int)x.TaxID)
                        .Distinct()
                        .ToList();

        var discountIds = orderDTO.Rows
                            .Where(y => y.DiscountID != null)
                            .Select(x => (int)x.DiscountID)
                            .Distinct()
                            .ToList();

        var requestValues = await _GetOrderRequestValuesAsync(
                                    orderDTO.EntityId,
                                    orderDTO.TemplateId,
                                    productIds,
                                    taxIds,
                                    discountIds);

        var billAddress = _mapper.Map<Address>(orderDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(orderDTO.ShippingAddress);

        _order = Order.Initialize(
                    requestValues.Entity,
                    requestValues.OrderTemplate,
                    billAddress,
                    shipAddress,
                    username);

        _AssignRowsToOrder(orderDTO.Rows, requestValues);

    }

    private void _AssignRowsToOrder(IEnumerable<CreateOrderRowDTO> orderDtoRows, OrderRequestValues requestValues)
    {
        var rowsList = new List<OrderRow>();
        foreach (var (rowDTO, rowIndex) in orderDtoRows.WithIndex())
        {
            var thisProduct = requestValues.Products.FirstOrDefault(x => x.Id == rowDTO.ProductId);

            var thisTax = rowDTO.TaxID != null ? requestValues.Taxes.FirstOrDefault(x => x.Id == rowDTO.TaxID) : null;
            var thisDiscount = rowDTO.DiscountID != null ? requestValues.Discounts.FirstOrDefault(x => x.Id == rowDTO.DiscountID) : null;

            var detail = new OrderRow(rowIndex + 1, thisProduct, rowDTO.ProductQuantity, decimal.Zero, rowDTO.UnitPrice, thisTax, thisDiscount);
            rowsList.Add(detail);
        }

        _order.Rows = rowsList;
    }

    private async Task<OrderRequestValues> _GetOrderRequestValuesAsync(
        int entityId,
        int templateId,
        List<int> productIds,
        List<int> taxIds,
        List<int> discountIds)
    {
        var template = await _api.OrderTemplatesRepo.GetByIdAsync(templateId);
        _ = template ?? throw new ThesisERPException($"order Template with id: '{templateId}' not found.");

        var entityType = template.UsesSupplierEntity ? EntityType.supplier : EntityType.client;
        var entity = (await _api.EntitiesRepo.GetAllAsync(x => x.Id == entityId && x.EntityType == entityType)).FirstOrDefault();
        _ = entity ?? throw new ThesisERPException($"{entityType} with id: '{entityId}' not found.");

        if (!productIds.Any()) { throw new ThesisERPException("A valid order rows list has to be provided."); }

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

        return new OrderRequestValues(entity, template, products, taxes, discounts);
    }

    private async Task _UpdatePendingOrderWithNewValues(UpdateOrderDTO updateOrderDTO)
    {
        var productIds = updateOrderDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid order row list has to be provided."); }

        var taxIds = updateOrderDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = updateOrderDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var entityId = updateOrderDTO.EntityId ?? throw new ThesisERPException($"EntityId has to be provided when updating a pending order.");


        var newRequestValues = await _GetOrderRequestValuesAsync(entityId, _order.TemplateId, productIds, taxIds, discountIds);

        _order.Entity = newRequestValues.Entity;

        _AssignRowsToOrder(updateOrderDTO.Rows, newRequestValues);
    }

    private void _ValidateFulfillmentRows(FulfillOrderDTO fulfillOrderDTO)
    {
        var rowNumbersToFulfill = fulfillOrderDTO.FullfillmentRows?.Select(x => x.LineNumber).ToList();

        if (rowNumbersToFulfill.IsNullOrEmpty()) { return; }

        if (rowNumbersToFulfill.Count != rowNumbersToFulfill.Distinct().Count())
        {
            throw new ThesisERPException("Fulfillment failed. Each line number provided should be unique when partially fulfilling an order.");
        }

        var nonExistingRows = rowNumbersToFulfill.Where(x => !_order.Rows.Select(r => r.ProductId).Contains(x));
        if (nonExistingRows.Any())
        {
            throw new ThesisERPException($"Fulfillment failed. The following line numbers do not exist in the order: [ {string.Join(",", nonExistingRows)} ]");
        }

        foreach (var row in fulfillOrderDTO.FullfillmentRows)
        {
            var orderRow = _order.Rows.FirstOrDefault(x => x.LineNumber == row.LineNumber);

            if (orderRow.FulfilledQuantity + row.QuantityToFulfill > orderRow.ProductQuantity)
            {
                throw new ThesisERPException($"Fulfillment failed. Quantity to fulfill exceeds remaining ordered quantity.");
            }
        }
    }

    public async Task<List<GenericOrderDTO>> GetOrders()
    {
        var orders = await _api.OrdersRepo
                                .GetAllAsync
                                 (orderBy: o => o.OrderByDescending(d => d.DateUpdated),
                                 include: i => i.Include(p => p.Entity)
                                                .Include(x => x.InventoryLocation)
                                                .Include(t => t.Template)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Product)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Tax)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Discount));

        var results = _mapper.Map<List<GenericOrderDTO>>(orders);

        return results;
    }

    public async Task<GenericOrderDTO?> GetOrder(int id)
    {     

        var order = await _api.OrdersRepo.GetOrderByIdIncludeRelations(id);

        if (order is null) { return null; }

        var result = _mapper.Map<GenericOrderDTO>(order);

        return result;
    }

    private class OrderRequestValues
    {
        public Entity Entity { get; set; }
        public OrderTemplate OrderTemplate { get; set; }
        public List<Product> Products { get; set; }
        public List<Tax> Taxes { get; set; }
        public List<Discount> Discounts { get; set; }

        public OrderRequestValues(Entity entity, OrderTemplate template, List<Product> products, List<Tax> taxes, List<Discount> discounts)
        {
            Entity = entity;
            OrderTemplate = template;
            Products = products;
            Taxes = taxes;
            Discounts = discounts;
        }
    }

}
