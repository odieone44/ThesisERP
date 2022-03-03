using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IOrderService 
{
    Task<GenericOrderDTO> Create(CreateOrderDTO createTransactionDTO, string username);
    Task<GenericOrderDTO> Update(int id, UpdateOrderDTO updateTransactionDTO);
    Task<GenericOrderDTO> Fulfill(int id, FulfillOrderDTO fulfillDTO);
    Task<GenericOrderDTO> Close(int id);
    Task<GenericOrderDTO> Cancel(int id);
}
