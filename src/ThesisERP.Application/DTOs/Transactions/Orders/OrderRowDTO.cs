namespace ThesisERP.Application.DTOs.Transactions.Orders;

public class CreateOrderRowDTO : CreateTransactionRowBaseDTO
{
}

public class OrderRowDTO : TransactionRowBaseDTO
{
    public decimal FulfilledQuantity { get; set; } = decimal.Zero;

    public bool RowIsFulfilled { get; set; }
}
