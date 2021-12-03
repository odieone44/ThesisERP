namespace ThesisERP.Core.Interfaces
{
    public interface ITransactionDetail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int? DiscountID { get; set; }
        public int? TaxID { get; set; }
        public decimal LineTotalTax { get; }
        public decimal LineTotalDiscount { get; }
        public decimal LineTotalNet { get; }
        public decimal LineTotalGross { get; }

    }
}
