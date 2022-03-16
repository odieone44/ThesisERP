using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IOrderService 
{
    Task<GenericOrderDTO> Create(CreateOrderDTO createOrderDTO, string username);
    Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateOrderDTO);
    Task<GenericOrderDTO> Process(int id, ProcessOrderDTO processOrderDTO);
    Task<GenericOrderDTO> Fulfill(int id, FulfillOrderDTO fulfillOrderDTO);
    Task<GenericOrderDTO> Close(int id);
    Task<GenericOrderDTO> Cancel(int id);

    Task<List<GenericOrderDTO>> GetOrders();
    Task<GenericOrderDTO?> GetOrder(int id);
}
