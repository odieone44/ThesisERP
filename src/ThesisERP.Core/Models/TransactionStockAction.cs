using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Models;

public readonly record struct TransactionStockAction(
    TransactionStatus OldStatus, 
    TransactionStatus NewStatus, 
    StockChangeType StockChangeType
)
{
    public bool IncreasesIncomingStock => GetActionsThatIncreaseIncomingStock().Contains(this);
    public bool DecreasesIncomingStock => GetActionsThatDecreaseIncomingStock().Contains(this);
    public bool IncreasesOutgoingStock => GetActionsThatIncreaseOutgoingStock().Contains(this);
    public bool DecreasesOutgoingStock => GetActionsThatDecreaseOutgoingStock().Contains(this);
    public bool IncreasesAvailableStock => GetActionsThatIncreaseAvailableStock().Contains(this);
    public bool DecreasesAvailableStock => GetActionsThatDecreaseAvailableStock().Contains(this);
    public bool HasStockEffect => GetActionsThatChangeStock().Contains(this);

    private static IEnumerable<TransactionStockAction> GetActionsThatChangeStock()
    {
        var actions = new List<TransactionStockAction>();

        actions.AddRange(GetActionsThatIncreaseIncomingStock());
        actions.AddRange(GetActionsThatDecreaseIncomingStock());
        actions.AddRange(GetActionsThatIncreaseOutgoingStock());
        actions.AddRange(GetActionsThatDecreaseOutgoingStock());
        actions.AddRange(GetActionsThatIncreaseAvailableStock());
        actions.AddRange(GetActionsThatDecreaseAvailableStock());

        return actions.Distinct();
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseIncomingStock()
    {
        yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.pending, StockChangeType.positive);
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseIncomingStock()
    {
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.cancelled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.draft, StockChangeType.positive);
    }
    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseOutgoingStock()
    {
        yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.pending, StockChangeType.negative);
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseOutgoingStock()
    {
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.cancelled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.draft, StockChangeType.negative);
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseAvailableStock()
    {
        yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.fulfilled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionStatus.fulfilled, TransactionStatus.cancelled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionStatus.closed, TransactionStatus.cancelled, StockChangeType.negative);
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseAvailableStock()
    {
        yield return new TransactionStockAction(TransactionStatus.draft, TransactionStatus.fulfilled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionStatus.pending, TransactionStatus.fulfilled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionStatus.fulfilled, TransactionStatus.cancelled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionStatus.closed, TransactionStatus.cancelled, StockChangeType.positive);
    }
};
