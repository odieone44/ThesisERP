using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs.Transactions.Orders;

public class CreateOrderRowDTO : CreateTransactionRowBaseDTO
{
}

public class OrderRowDTO : TransactionRowBaseDTO
{
    public decimal FulfilledQuantity { get; set; } = decimal.Zero;

    public bool RowIsFulfilled { get; set; }
}

public class FulfillOrderRowDTO
{
    [Range(1, 1000, ErrorMessage = "Must be a positive value")]
    public int LineNumber { get; set; }
    
    [Range(0.0001, double.MaxValue, ErrorMessage = "Must be a positive value")]
    public decimal QuantityToFulfill { get; set;} = decimal.Zero;
}