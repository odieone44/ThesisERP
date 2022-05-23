using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IOrderService 
{
    Task<GenericOrderDTO> CreateAsync(CreateOrderDTO createOrderDTO, string username);
    Task<GenericOrderDTO> UpdateAsync(int id, UpdateOrderDTO updateOrderDTO);
    Task<GenericOrderDTO> ProcessAsync(int id, ProcessOrderDTO processOrderDTO);
    Task<GenericOrderDTO> FulfillAsync(int id, FulfillOrderDTO fulfillOrderDTO);
    Task<GenericOrderDTO> CloseAsync(int id);
    Task<GenericOrderDTO> CancelAsync(int id);

    Task<List<GenericOrderDTO>> GetOrdersAsync();
    Task<GenericOrderDTO?> GetOrderAsync(int id);
}
