using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Document
{
    public int Id { get; set; }

    public int EntityId { get; set; }
    public Entity Entity { get; set; }

    public int InventoryLocationId { get; set; }
    public InventoryLocation InventoryLocation { get; set; }

    public int TemplateId { get; set; }
    public DocumentTemplate DocumentTemplate { get; set; }

    public string DocumentNumber { get; set; }

    public TransactionStatus Status { get; set; }

    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }

    public string Comments { get; set; } = string.Empty;

    public ICollection<DocumentRow> Rows { get; set; } = new List<DocumentRow>();

    public byte[] Timestamp { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public string CreatedBy { get; set; }
    public DocumentType Type => DocumentTemplate.DocumentType;
    public bool IsFulfilled => Status == TransactionStatus.fulfilled;
    public bool IsClosed => Status == TransactionStatus.closed;
    public bool IsCancelled => Status == TransactionStatus.cancelled;

    private Document() { }

    public static Document Initialize(Entity entity,
                                      InventoryLocation location,
                                      DocumentTemplate template,
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
            DocumentTemplate = template,
            TemplateId = template.Id,
            BillingAddress = billingAddress,
            ShippingAddress = shippingAddress,
            DocumentNumber = $"{template.Prefix}{template.NextNumber}{template.Postfix}",
            Status = TransactionStatus.draft,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
            CreatedBy = username
        };
    }
}
