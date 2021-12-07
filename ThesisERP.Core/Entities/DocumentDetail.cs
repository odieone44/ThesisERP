using ThesisERP.Core.Extensions;

namespace ThesisERP.Core.Entities;

#pragma warning disable CS8618

public class DocumentDetail
{

    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }

    private decimal _productQuantity;
    public decimal ProductQuantity { get => _productQuantity; set => _productQuantity = value.RoundTo(4); }

    private decimal _unitPrice = decimal.Zero;
    public decimal UnitPrice { get => _unitPrice; set => _unitPrice = value.RoundTo(2); }


    public int? DiscountID { get; set; }
    public Discount? Discount { get; set; }


    public int? TaxID { get; set; }
    public Tax? Tax { get; set; }


    public int ParentDocumentId { get; set; }
    public Document ParentDocument { get; set; }

    public decimal LineTotalTax { get; private set; } = decimal.Zero;
    public decimal LineTotalDiscount { get; private set; } = decimal.Zero;

    public byte[] Timestamp { get; private set; }

    private decimal _lineTotalNet;
    public decimal LineTotalNet
    {
        get => _lineTotalNet;
        private set => _lineTotalNet = value.RoundTo(2);
    }

    private decimal _lineTotalGross;
    public decimal LineTotalGross
    {
        get => _lineTotalGross;
        private set => _lineTotalGross = value.RoundTo(2);
    }


    public DocumentDetail(Product product, decimal quantity, decimal price, Tax? tax = null, Discount? discount = null)
    {
        Product = product;
        Discount = discount;
        Tax = tax;
        ProductQuantity = quantity;
        UnitPrice = price;

        LineTotalNet = quantity * price;
        LineTotalTax = tax == null ? decimal.Zero : (price * tax.Amount * quantity).RoundTo(2);
        LineTotalDiscount = discount == null ? decimal.Zero : (price * discount.Amount * quantity).RoundTo(2);

        LineTotalGross = LineTotalNet + LineTotalTax - LineTotalDiscount;

    }


    public DocumentDetail()
    {

    }

}
#pragma warning restore CS8618

