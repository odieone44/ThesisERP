using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Interfaces;

public interface ITransaction
{
    Transactions.TransactionStatus Status { get; set; }
    Transactions.TransactionType Type { get; }
    public InventoryLocation InventoryLocation { get; set; }
    public TransactionTemplate TransactionTemplate { get; set; }
    ICollection<DocumentDetail> Details { get; set; }
    bool IsFulfilled { get; }
    bool IsClosed { get; }
    bool IsCancelled { get; }
    bool IsPositiveStockTransaction { get; }
}
