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
        
        public decimal ProductQuantity { get; set; }
        public decimal UnitPrice { get; set; } = decimal.Zero;
        
        [ForeignKey(nameof(Discount))]
        public int? DiscountID { get; set; }
        public Discount? Discount { get; set; }
        
        [ForeignKey(nameof(Tax))]
        public int? TaxID { get; set; }        
        public Tax? Tax { get; set; }
        
        [ForeignKey(nameof(ParentDocument))]
        public int ParentDocumentId { get; set; }
        public Document ParentDocument { get; set; }
        
        public decimal LineTotalTax { get; set; } = decimal.Zero;
        public decimal LineTotalDiscount { get; set; } = decimal.Zero;

        [Timestamp]
        public byte[] Timestamp { get; set; }

        private decimal _lineTotalNet;
        public decimal LineTotalNet
        {
            get => (UnitPrice * ProductQuantity).RoundTo2();
            private set => _lineTotalNet = (UnitPrice * ProductQuantity).RoundTo2();
        }
        public decimal LineTotalGross => (LineTotalNet + LineTotalTax - LineTotalDiscount).RoundTo2();

    }
}
