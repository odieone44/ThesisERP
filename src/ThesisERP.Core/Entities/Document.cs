using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Document : TransactionBase
{   
    public int InventoryLocationId { get; set; }
    public InventoryLocation InventoryLocation { get; set; }
    public int TemplateId { get; set; }
    public DocumentTemplate DocumentTemplate { get; set; }
    public string DocumentNumber { get; set; }
    public ICollection<DocumentRow> Rows { get; set; } = new List<DocumentRow>();
    public byte[] Timestamp { get; set; }
    public DocumentType Type => DocumentTemplate.DocumentType;
 
    private Document() : base() { }

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
