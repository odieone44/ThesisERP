using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Models.Stock;

public readonly record struct TransactionStockAction(TransactionAction Action, TransactionStatus OldStatus, StockChangeType StockChangeType);

public class StockLevelUpdateHelper
{
    private readonly decimal _amount;
    private readonly TransactionStockAction _stockAction;

    public StockLevelUpdateHelper(TransactionStockAction transStockAction, decimal updateAmount)
    {
        _amount = updateAmount;
        _stockAction = transStockAction;        
    }

    public void HandleStockLevelUpdate(StockLevel stockEntry)
    {
        if (GetActionsThatIncreaseIncomingStock().Contains(_stockAction))
        {
            stockEntry.Incoming += _amount;
        }
        else if (GetActionsThatDecreaseIncomingStock().Contains(_stockAction))
        {
            stockEntry.Incoming -= _amount;
        }

        if (GetActionsThatIncreaseOutgoingStock().Contains(_stockAction))
        {
            stockEntry.Outgoing += _amount;
        }
        else if (GetActionsThatDecreaseOutgoingStock().Contains(_stockAction))
        {
            stockEntry.Outgoing -= _amount;
        }

        if (GetActionsThatIncreaseAvailableStock().Contains(_stockAction))
        {
            stockEntry.Available += _amount;
        }
        else if (GetActionsThatDecreaseAvailableStock().Contains(_stockAction))
        {
            stockEntry.Available -= _amount;
            if (stockEntry.Available < 0)
            {
                throw new ThesisERPException($"Cannot complete transaction as it will result in negative stock for product '{stockEntry.Product.SKU}' in location '{stockEntry.InventoryLocation.Name}'");
            }
        }
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseIncomingStock()
    {
        yield return new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, StockChangeType.positive);       
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseIncomingStock()
    {
        yield return new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, StockChangeType.positive);        
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseOutgoingStock()
    {
        yield return new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, StockChangeType.negative);      
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseOutgoingStock()
    {
        yield return new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, StockChangeType.negative);
    }


    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseAvailableStock()
    {
        yield return new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, StockChangeType.negative);
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, StockChangeType.negative);        
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseAvailableStock()
    {
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, StockChangeType.positive);
        yield return new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, StockChangeType.negative);        
    }

}


