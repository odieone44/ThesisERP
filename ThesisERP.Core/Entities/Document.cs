using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Entities;

namespace ThesisERP.Core.Entities
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

        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public ICollection<DocumentDetail> Details { get; set; } = new List<DocumentDetail>();       

        public byte[] Timestamp { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
