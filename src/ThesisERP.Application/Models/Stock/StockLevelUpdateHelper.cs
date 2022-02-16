using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Models.Stock;

public readonly record struct TransactionStockAction(TransactionAction Action, TransactionStatus OldStatus, DocumentType Type);

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
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetPositiveStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseIncomingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetPositiveStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseOutgoingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetNegativeStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.create, TransactionStatus.draft, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseOutgoingStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetNegativeStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.pending, type));
        }
        return actionList;
    }


    private static IEnumerable<TransactionStockAction> GetActionsThatIncreaseAvailableStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetPositiveStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
        }
        foreach (var type in DocumentTemplate.GetNegativeStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, type));
        }
        return actionList;
    }

    private static IEnumerable<TransactionStockAction> GetActionsThatDecreaseAvailableStock()
    {
        var actionList = new List<TransactionStockAction>();
        foreach (var type in DocumentTemplate.GetPositiveStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.fulfilled, type));
            actionList.Add(new TransactionStockAction(TransactionAction.cancel, TransactionStatus.closed, type));
        }
        foreach (var type in DocumentTemplate.GetNegativeStockChangeDocumentTypes())
        {
            actionList.Add(new TransactionStockAction(TransactionAction.fulfill, TransactionStatus.pending, type));
        }
        return actionList;
    }

}


