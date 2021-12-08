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
    public Transactions.Types TransactionType { get; set; }
    public byte[] Timestamp { get; set; }

    public Transactions.StockChangeTypes GetStockChangeType()
    {
        return GetPositiveTransactionTypes().Contains(TransactionType) ? Transactions.StockChangeTypes.positive : Transactions.StockChangeTypes.negative;
    }

    public static IEnumerable<Transactions.Types> GetPositiveTransactionTypes()
    {
        yield return Transactions.Types.purchase_order;
        yield return Transactions.Types.purchase_bill;
        yield return Transactions.Types.sales_return;
        yield return Transactions.Types.stock_adjustment_plus;
    }

    public static IEnumerable<Transactions.Types> GetNegativeTransactionTypes()
    {
        yield return Transactions.Types.sales_order;
        yield return Transactions.Types.sales_invoice;
        yield return Transactions.Types.purchase_return;
        yield return Transactions.Types.stock_adjustment_minus;
    }

}
