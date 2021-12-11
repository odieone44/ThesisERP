using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;
using System.Xml.Linq;

namespace ThesisERP.Core.Entities;

public class Document : ITransaction
{
    public int Id { get; set; }

    public int EntityId { get; set; }
    public Entity Entity { get; set; }

    public int InventoryLocationId { get; set; }
    public InventoryLocation InventoryLocation { get; set; }

    public int TemplateId { get; set; }
    public TransactionTemplate TransactionTemplate { get; set; }

    public string DocumentNumber { get; set; }

    public Transactions.TransactionStatus Status { get; set; }

    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }

    public string Comments { get; set; } = string.Empty;

    public ICollection<DocumentDetail> Details { get; set; } = new List<DocumentDetail>();

    public byte[] Timestamp { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public string CreatedBy { get; set; }
    public Transactions.TransactionType Type => TransactionTemplate.TransactionType;
    public bool IsFulfilled => Status == Transactions.TransactionStatus.fulfilled;
    public bool IsClosed => Status == Transactions.TransactionStatus.closed;
    public bool IsCancelled => Status == Transactions.TransactionStatus.cancelled;   

    private Document() { }   

    public static Document Initialize(Entity entity,
                                      InventoryLocation location,
                                      TransactionTemplate template,
                                      Address billingAddress,
                                      Address shippingAddress, 
                                      string username)
    {
        return new()
        {
            Entity = entity,
            EntityId = entity.Id,
            InventoryLocation = location,
            InventoryLocationId = location.Id,
            TransactionTemplate = template,
            TemplateId = template.Id,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            DocumentNumber = $"{template.Prefix}{template.NextNumber}{template.Postfix}",
            Status = Transactions.TransactionStatus.draft,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
            CreatedBy = username
        };
    }
}
