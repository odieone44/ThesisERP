using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs.Transactions;


public abstract class CreateTransactionRowBaseDTO
{
    public int ProductId { get; set; }

    [Range(0.0001, double.MaxValue, ErrorMessage = "Must be a positive value")]
    public decimal ProductQuantity { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Must be a positive value")]
    public decimal UnitPrice { get; set; }
    public int? DiscountID { get; set; }
    public int? TaxID { get; set; }
}

public abstract class TransactionRowBaseDTO
{
    public int LineNumber { get; set; }
    public int ProductId { get; set; }
    public string ProductSKU { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? DiscountID { get; set; }
    public string? DiscountName { get; set; }
    public int? TaxID { get; set; }
    public string? TaxName { get; set; }
    public decimal LineTotalTax { get; set; } = decimal.Zero;
    public decimal LineTotalDiscount { get; set; } = decimal.Zero;
    public decimal LineTotalNet { get; set; } = decimal.Zero;
    public decimal LineTotalGross { get; set; } = decimal.Zero;
}
