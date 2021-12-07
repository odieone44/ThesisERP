using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;

namespace ThesisERP.Core.Entities
{

    public class Document
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }

        public int InventoryLocationId { get; set; }
        public InventoryLocation InventoryLocation { get; set; }

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

        public string CreatedBy { get; set; }

        public Document() { }

        public static Document CreateSalesDocument(Entity entity, 
                                                   InventoryLocation location, 
                                                   TransactionTemplate template,
                                                   Address billingAddress, 
                                                   Address shippingAddress, 
                                                   List<DocumentDetail> details,
                                                   string username)
        {
            if (entity.EntityType != Enums.Entities.EntityTypes.client)
            {
                throw new ThesisERPException($"Entity needs to be of type '{Enums.Entities.EntityTypes.client}' for this document template.");
            }

            return new Document()
            {
                Entity = entity,
                InventoryLocation = location,
                TransactionTemplate = template,
                DocumentNumber = $"{template.Prefix}{template.NextNumber}{template.Postfix}",
                Status = Transactions.Status.pending,
                Details = details,
                BillingAddress = billingAddress,
                ShippingAddress = shippingAddress,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CreatedBy = username
            };
        }
    }
}
