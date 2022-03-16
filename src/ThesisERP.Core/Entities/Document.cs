using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Document : TransactionBase<DocumentTemplate,DocumentRow>
{   
    public int InventoryLocationId { get; set; }
    public InventoryLocation InventoryLocation { get; set; }    
    public int? ParentOrderId { get; set; }
    public Order? ParentOrder { get; set; }
    public DocumentType Type => Template.DocumentType;
    public bool CanBeUpdated => GetDocumentStatusesThatCanBeUpdated().Contains(Status);

    private Document() : base() { }

    private Document(Entity entity,
                     InventoryLocation location,
                     DocumentTemplate template,
                     Address billingAddress,
                     Address shippingAddress,
                     Order? parentOrder,
                     string username) 
        : base(entity, template, billingAddress, shippingAddress, username) 
    {     
        InventoryLocation = location;
        InventoryLocationId = location.Id;
        ParentOrderId = parentOrder?.Id;
        ParentOrder = parentOrder;    
    }

    public static Document Initialize(Entity entity,
                                      InventoryLocation location,
                                      DocumentTemplate template,
                                      Address billingAddress,
                                      Address shippingAddress,
                                      Order? parentOrder,
                                      string username)
    {        
        return new(entity, location, template, billingAddress, shippingAddress, parentOrder, username);        
    }

    private static IEnumerable<TransactionStatus> GetDocumentStatusesThatCanBeUpdated()
    {
        yield return TransactionStatus.draft;
        yield return TransactionStatus.pending;
        yield return TransactionStatus.fulfilled;
    }


}
