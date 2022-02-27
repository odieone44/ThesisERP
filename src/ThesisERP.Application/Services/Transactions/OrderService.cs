using AutoMapper;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Transactions;

public class OrderService : IOrderService
{
    private readonly IRepositoryBase<Order> _ordersRepo;
    private readonly IRepositoryBase<Product> _productsRepo;
    private readonly IRepositoryBase<OrderTemplate> _templatesRepo;
    private readonly IRepositoryBase<Entity> _entitiesRepo;
    private readonly IRepositoryBase<InventoryLocation> _locationsRepo;
    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IRepositoryBase<Tax> _taxRepo;
    private readonly IRepositoryBase<Discount> _discountRepo;
    private readonly IMapper _mapper;

    public Task<GenericOrderDTO> Cancel(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Close(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Create(CreateOrderDTO createTransactionDTO, string username)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Fulfill(int id)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateTransactionDTO)
    {
        throw new NotImplementedException();
    }
}
