using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Exceptions;

namespace ThesisERP.Core.Entities;

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

    public string Comments { get; set; } = string.Empty;

    public ICollection<DocumentDetail> Details { get; set; } = new List<DocumentDetail>();

    public byte[] Timestamp { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public string CreatedBy { get; set; }

    private Document() { }

    public Document(Entity entity,
                    InventoryLocation location,
                    TransactionTemplate template,
                    Address billingAddress,
                    Address shippingAddress,
                    List<DocumentDetail> details,
                    string comments,
                    string username)
    {
        Entity = entity;
        InventoryLocation = location;
        TransactionTemplate = template;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        DocumentNumber = $"{template.Prefix}{template.NextNumber}{template.Postfix}";
        Status = Transactions.Status.pending;
        Details = details;
        DateCreated = DateTime.UtcNow;
        DateUpdated = DateTime.UtcNow;
        CreatedBy = username;
        Comments = comments;
    }
}
