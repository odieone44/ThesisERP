using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class DocumentTemplate : TransactionTemplateBase
{
    public DocumentType DocumentType { get; set; }

    public override StockChangeType StockChangeType => GetStockChangeType();

    public bool UsesClientEntity => GetDocumentEntityType() == EntityType.client;
    public bool UsesSupplierEntity => !UsesClientEntity;

    private StockChangeType GetStockChangeType()
    {
        return GetPositiveStockChangeDocumentTypes().Contains(DocumentType) ? StockChangeType.positive : StockChangeType.negative;
    }

    private EntityType GetDocumentEntityType()
    {
        return GetSupplierDocumentTypes().Contains(DocumentType) ? EntityType.supplier : EntityType.client;
    }

    public static IEnumerable<DocumentType> GetSupplierDocumentTypes()
    {
        yield return DocumentType.purchase_bill;
        //yield return DocumentType.purchase_return;
        yield return DocumentType.stock_adjustment_plus;
    }

    public static IEnumerable<DocumentType> GetClientDocumentTypes()
    {
        yield return DocumentType.sales_invoice;
        //yield return DocumentType.sales_return;
        yield return DocumentType.stock_adjustment_minus;
    }

    public static IEnumerable<DocumentType> GetPositiveStockChangeDocumentTypes()
    {
        yield return DocumentType.purchase_bill;
        //yield return DocumentType.sales_return;
        yield return DocumentType.stock_adjustment_plus;
    }

    public static IEnumerable<DocumentType> GetNegativeStockChangeDocumentTypes()
    {
        yield return DocumentType.sales_invoice;
        //yield return DocumentType.purchase_return;
        yield return DocumentType.stock_adjustment_minus;
    }

}
