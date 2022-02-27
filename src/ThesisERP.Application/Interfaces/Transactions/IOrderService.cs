using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IOrderService : 
    ITransactionService<GenericOrderDTO, CreateOrderDTO, UpdateOrderDTO, OrderRowDTO, CreateOrderRowDTO>
{
}
