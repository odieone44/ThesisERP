using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Models;

public readonly record struct TransactionStockAction(
    TransactionStatus OldStatus, 
    TransactionStatus NewStatus, 
    StockChangeType StockChangeType
);
