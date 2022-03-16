using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    private readonly IMapper _mapper;

    private Order _order;

    public OrderService(IApiService apiService,
                        IMapper mapper)
    {
        _api = apiService;
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

    public Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateOrderDTO)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Process(int id, ProcessOrderDTO processOrderDTO)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Fulfill(int id, FulfillOrderDTO fulfillOrderDTO)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Close(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Cancel(int id)
    {
        throw new NotImplementedException();
    }

    private async Task _InitializeNewOrder(CreateOrderDTO orderDTO, string username)
    {
        var productIds = orderDTO.Rows.Select(x => x.ProductId).Distinct().ToList();
        if (!productIds.Any()) { throw new ThesisERPException("A valid order rows list has to be provided."); }

        var taxIds = orderDTO.Rows.Where(y => y.TaxID != null).Select(x => (int)x.TaxID).Distinct().ToList();
        var discountIds = orderDTO.Rows.Where(y => y.DiscountID != null).Select(x => (int)x.DiscountID).Distinct().ToList();

        var requestValues = await _GetOrderRequestValuesAsync(orderDTO.EntityId, orderDTO.TemplateId, productIds, taxIds, discountIds);

        var billAddress = _mapper.Map<Address>(orderDTO.BillingAddress);
        var shipAddress = _mapper.Map<Address>(orderDTO.ShippingAddress);

        _order = Order.Initialize(
            requestValues.Entity,
            requestValues.OrderTemplate,
            billAddress,
            shipAddress,
            username
        );

        var rowsList = new List<OrderRow>();
        foreach (var (rowDTO, rowIndex) in orderDTO.Rows.WithIndex())
        {
            var thisProduct = requestValues.Products.FirstOrDefault(x => x.Id == rowDTO.ProductId);

            var thisTax = rowDTO.TaxID != null ? requestValues.Taxes.FirstOrDefault(x => x.Id == rowDTO.TaxID) : null;
            var thisDiscount = rowDTO.DiscountID != null ? requestValues.Discounts.FirstOrDefault(x => x.Id == rowDTO.DiscountID) : null;

            var detail = new OrderRow(rowIndex, thisProduct, rowDTO.ProductQuantity, decimal.Zero, rowDTO.UnitPrice, thisTax, thisDiscount);
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
