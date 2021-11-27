using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Data.Interfaces;
using ThesisERP.Static.Extensions;

namespace ThesisERP.Data
{
    [Owned]
    [Table("DocumentDetails")]
    public class DocumentDetail : ITransactionDetail
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        private decimal _productQuantity;
        public decimal ProductQuantity {get => _productQuantity; set => _productQuantity = value.RoundTo(4); }
        
        private decimal _unitPrice = decimal.Zero;
        public decimal UnitPrice { get => _unitPrice; set => _unitPrice = value.RoundTo(2); }
        
        [ForeignKey(nameof(Discount))]
        public int? DiscountID { get; set; }
        public Discount? Discount { get; set; }
        
        [ForeignKey(nameof(Tax))]
        public int? TaxID { get; set; }        
        public Tax? Tax { get; set; }
        
        [ForeignKey(nameof(ParentDocument))]
        public int ParentDocumentId { get; set; }
        public Document ParentDocument { get; set; }
        
        public decimal LineTotalTax { get; private set; } = decimal.Zero;
        public decimal LineTotalDiscount { get; private set; } = decimal.Zero;

        [Timestamp]
        public byte[] Timestamp { get; set; }

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
            this.Product = product;
            this.Discount = discount;
            this.Tax = tax;
            this.ProductQuantity = quantity;
            this.UnitPrice = price;

            this.LineTotalNet = quantity * price;
            this.LineTotalTax = tax == null ? decimal.Zero : (price * tax.Amount * quantity).RoundTo(2);
            this.LineTotalDiscount = discount == null ? decimal.Zero : (price * discount.Amount * quantity).RoundTo(2);

            this.LineTotalGross = LineTotalNet + LineTotalTax - LineTotalDiscount;

        }

        public DocumentDetail()
        {

        }

    }
}
