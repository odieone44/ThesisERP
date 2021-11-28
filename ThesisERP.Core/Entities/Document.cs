using ThesisERP.Core.Interfaces;
using ThesisERP.Static.Enums;

namespace ThesisERP.Core.Entites
{

    public class Document : ITransaction
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }


        public int TemplateId { get; set; }
        public TransactionTemplate TransactionTemplate { get; set; }

        public string DocumentNumber { get; set; }
        string ITransaction.Number
        {
            get { return DocumentNumber; }
            set { DocumentNumber = value; }
        }

        public Transactions.Status Status { get; set; }
        public ICollection<DocumentAddress> DocumentAddresses { get; set; } = new List<DocumentAddress>();
        ICollection<IAddress> ITransaction.TransactionAddresses
        {
            get { return DocumentAddresses.Cast<IAddress>().ToList(); }
            set { DocumentAddresses = value.Cast<DocumentAddress>().ToList(); }
        }

        public DocumentAddress BillingAddress => DocumentAddresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.billiing);
        public DocumentAddress ShippingAddress => DocumentAddresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.shipping);

        public ICollection<DocumentDetail> Details { get; set; } = new List<DocumentDetail>();
        ICollection<ITransactionDetail> ITransaction.Details
        {
            get { return Details.Cast<ITransactionDetail>().ToList(); }
            set { Details = value.Cast<DocumentDetail>().ToList(); }
        }

        public byte[] Timestamp { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
