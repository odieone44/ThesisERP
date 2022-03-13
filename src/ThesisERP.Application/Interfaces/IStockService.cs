using ThesisERP.Application.DTOs;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Models;

namespace ThesisERP.Application.Interfaces;

public interface IStockService
{
    Task HandleStockUpdateForTransactionRows(
        InventoryLocation location,
        IEnumerable<TransactionRowBase> transactionRows,
        TransactionStockAction stockAction);

    Task<List<GetLocationStockDTO>> GetLocationStock(int? locationId);

    Task<List<GetProductStockDTO>> GetProductStock(int? productId);

}
