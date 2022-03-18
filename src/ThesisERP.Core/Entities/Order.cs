using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Order : TransactionBase<OrderTemplate, OrderRow>
{
    public int? InventoryLocationId { get; set; }
    public InventoryLocation? InventoryLocation { get; set; }
    public ICollection<Document> RelatedDocuments { get; set; } = new List<Document>();
    public OrderType Type => Template.OrderType;
    public bool IsPartiallyFulfilled => Rows.Count > Rows.Where(row => row.RowIsFulfilled).Count() && Rows.Where(row => row.RowIsFulfilled).Any();

    public override bool CanBeUpdated => GetOrderStatusesThatCanBeUpdated().Contains(Status);

    private Order() : base() { }

    private Order(
        Entity entity,
        OrderTemplate template,
        Address billingAddress,
        Address shippingAddress,
        string username)
        : base(entity, template, billingAddress, shippingAddress, username)
    {
    }

    public static Order Initialize(
        Entity entity,
        OrderTemplate template,
        Address billingAddress,
        Address shippingAddress,
        string username)
    {
        return new Order(entity, template, billingAddress, shippingAddress, username);
    }

    public bool CanBeFulfilledBy(DocumentType documentType)
    {
        return GetAllowedFulfillmentTypes().Contains(documentType);
    }

    private static IEnumerable<TransactionStatus> GetOrderStatusesThatCanBeUpdated()
    {
        yield return TransactionStatus.draft;
        yield return TransactionStatus.pending;
        yield return TransactionStatus.processing;
        yield return TransactionStatus.fulfilled;
    }

    private IEnumerable<DocumentType> GetAllowedFulfillmentTypes()
    {
        if (Type == OrderType.purchase_order)
        {
            yield return DocumentType.purchase_bill;
            //todo

        }
        else if (Type == OrderType.sales_order)
        {
            yield return DocumentType.sales_invoice;
            //todo
        }
    }


}
