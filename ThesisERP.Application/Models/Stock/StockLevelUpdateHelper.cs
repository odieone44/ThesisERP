using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;
using static ThesisERP.Core.Enums.Transactions;

namespace ThesisERP.Application.Models.Stock;

public readonly record struct TransactionStockAction(TransactionAction Action, TransactionStatus OldStatus, TransactionType Type);

public class StockLevelUpdateHelper
{
    private readonly decimal _amount;
    private readonly TransactionStockAction _stockAction;

    private bool _increaseIncoming = false;
    private bool _decreaseIncoming = false;
    private bool _increaseOutgoing = false;
    private bool _decreaseOutgoing = false;
    private bool _increaseAvailable = false;
    private bool _decreaseAvailable = false;

    public StockLevelUpdateHelper(TransactionStockAction transStockAction, decimal updateAmount)
    {
        _amount = updateAmount;
        _stockAction = transStockAction;
        _InitializeStockPropertiesToBeUpdated();
    }

    public void HandleStockLevelUpdate(StockLevel stockEntry)
    {
        if (_increaseIncoming)
        {
            stockEntry.Incoming += _amount;
        }
        else if (_decreaseIncoming)
        {
            stockEntry.Incoming -= _amount;
        }

        if (_increaseOutgoing)
        {
            stockEntry.Outgoing += _amount;
        }
        else if (_decreaseOutgoing)
        {
            stockEntry.Outgoing -= _amount;
        }

        if (_increaseAvailable)
        {
            stockEntry.Available += _amount;
        }
        else if (_decreaseAvailable)
        {
            stockEntry.Available -= _amount;
            if (stockEntry.Available < 0)
            {
                throw new ThesisERPException($"Cannot complete transaction as it will result in negative stock for product '{stockEntry.Product.SKU}' in location '{stockEntry.InventoryLocation.Name}'");
            }
        }
    }

    private void _InitializeStockPropertiesToBeUpdated()
    {
        if (GetActionsThatIncreaseIncomingStock().Contains(_stockAction))
        {
            _increaseIncoming = true;
        }
        else if (GetActionsThatDecreaseIncomingStock().Contains(_stockAction))
        {
            _decreaseIncoming = true;
        }

        if (GetActionsThatIncreaseOutgoingStock().Contains(_stockAction))
        {
            _increaseOutgoing = true;
        }
        else if (GetActionsThatDecreaseOutgoingStock().Contains(_stockAction))
        {
            _decreaseOutgoing = true;
        }

        if (GetActionsThatIncreaseAvailableStock().Contains(_stockAction))
        {
            _increaseAvailable = true;
        }
        else if (GetActionsThatDecreaseAvailableStock().Contains(_stockAction))
        {
            _decreaseAvailable = true;
        }
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseIncomingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetPositiveTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseIncomingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetPositiveTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseOutgoingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetNegativeTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseOutgoingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetNegativeTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, type));
        }
        return actionList;
    }


    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseAvailableStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetPositiveTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
        }
        foreach (var type in TransactionTemplate.GetNegativeTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseAvailableStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in TransactionTemplate.GetPositiveTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, type));
        }
        foreach (var type in TransactionTemplate.GetNegativeTransactionTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
        }
        return actionList;
    }

}


