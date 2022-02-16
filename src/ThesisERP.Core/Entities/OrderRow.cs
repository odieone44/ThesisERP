namespace ThesisERP.Core.Entities;
public class OrderRow : TransactionRowBase
{
    public int ParentOrderId { get; set; }
    public Order ParentOrder { get; set; }

    public decimal FulfilledQuantity { get; set; } = decimal.Zero;

    public bool RowIsFulfilled => ProductQuantity == FulfilledQuantity;

    public byte[] Timestamp { get; private set; }

    public OrderRow(Product product, decimal quantity, decimal fulfilledQuantity, decimal price, Tax? tax = null, Discount? discount = null)
        : base(product, quantity, price, tax, discount)
    {
        FulfilledQuantity = fulfilledQuantity;
    }

    private OrderRow() { }
}
