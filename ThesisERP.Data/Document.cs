using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Data.Interfaces;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data
{
    [Table("Documents")]
    public class Document : ITransaction
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Entity))]
        public int EntityId { get; set; }
        public Entity Entity { get; set; }

        [ForeignKey(nameof(TransactionTemplate))]
        public int TemplateId { get; set; }
        public TransactionTemplate TransactionTemplate { get; set; }

        public string DocumentNumber { get; set; }
        string ITransaction.Number
        {
            get { return DocumentNumber; }
            set { DocumentNumber = value; }
        }
        
        public Transactions.Status Status { get; set; }
        public ICollection<DocumentAddress> DocumentAddresses { get; set; }
        ICollection<IAddress> ITransaction.TransactionAddresses
        {
            get { return DocumentAddresses.Cast<IAddress>().ToList(); }
            set { DocumentAddresses = value.Cast<DocumentAddress>().ToList(); }
        }

        public DocumentAddress BillingAddress => DocumentAddresses.FirstOrDefault(add=>add.AddressType == Addresses.AddressTypes.billiing);
        public DocumentAddress ShippingAddress => DocumentAddresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.shipping);
        
        public ICollection<DocumentDetail> Details { get; set; }
        ICollection<ITransactionDetail> ITransaction.Details
        {
            get { return Details.Cast<ITransactionDetail>().ToList(); }
            set { Details = value.Cast<DocumentDetail>().ToList();}
        }

        [Timestamp]
        public byte[] Timestamp { get; set; }
        
    }
}
