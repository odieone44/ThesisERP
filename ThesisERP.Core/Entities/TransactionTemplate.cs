using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class TransactionTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Abbreviation { get; set; }
    public string Prefix { get; set; }
    public string Postfix { get; set; }
    public long NextNumber { get; set; } = 1;
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public Transactions.TransactionType TransactionType { get; set; }
    public byte[] Timestamp { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsPositiveStockTransaction => GetStockChangeType() == Transactions.StockChangeType.positive;

    private Transactions.StockChangeType GetStockChangeType()
    {
        return GetPositiveTransactionTypes().Contains(TransactionType) ? Transactions.StockChangeType.positive : Transactions.StockChangeType.negative;
    }

    public static IEnumerable<Transactions.TransactionType> GetPositiveTransactionTypes()
    {
        yield return Transactions.TransactionType.purchase_order;
        yield return Transactions.TransactionType.purchase_bill;
        yield return Transactions.TransactionType.sales_return;
        yield return Transactions.TransactionType.stock_adjustment_plus;
    }

    public static IEnumerable<Transactions.TransactionType> GetNegativeTransactionTypes()
    {
        yield return Transactions.TransactionType.sales_order;
        yield return Transactions.TransactionType.sales_invoice;
        yield return Transactions.TransactionType.purchase_return;
        yield return Transactions.TransactionType.stock_adjustment_minus;
    }

}
