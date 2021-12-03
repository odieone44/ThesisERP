using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entites
{

    public class Document
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }


        public int TemplateId { get; set; }
        public TransactionTemplate TransactionTemplate { get; set; }

        public string DocumentNumber { get; set; }        

        public Transactions.Status Status { get; set; }
        public ICollection<DocumentAddress> DocumentAddresses { get; set; } = new List<DocumentAddress>();       

        public DocumentAddress BillingAddress => DocumentAddresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.billiing);
        public DocumentAddress ShippingAddress => DocumentAddresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.shipping);

        public ICollection<DocumentDetail> Details { get; set; } = new List<DocumentDetail>();       

        public byte[] Timestamp { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
