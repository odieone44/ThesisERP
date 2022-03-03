using AutoMapper;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Transactions;

public class OrderService : IOrderService
{
    private readonly IApiService _api;
    private readonly IMapper _mapper;

    public OrderService(IApiService apiService,
                        IMapper mapper)
    {
        _api = apiService;
        _mapper = mapper;
    }

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

    public Task<GenericOrderDTO> Fulfill(int id, FulfillOrderDTO fulfillDTO)
    {
        throw new NotImplementedException();
    }

    public Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateTransactionDTO)
    {
        throw new NotImplementedException();
    }
}
