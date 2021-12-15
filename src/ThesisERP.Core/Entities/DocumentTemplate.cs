using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class DocumentTemplate
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Abbreviation { get; set; }
    public string Prefix { get; set; }
    public string Postfix { get; set; }
    public long NextNumber { get; set; } = 1;
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public DocumentType DocumentType { get; set; }
    public byte[] Timestamp { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsPositiveStockTransaction => GetStockChangeType() == StockChangeType.positive;

    private StockChangeType GetStockChangeType()
    {
        return GetPositiveStockChangeDocumentTypes().Contains(DocumentType) ? StockChangeType.positive : StockChangeType.negative;
    }

    public static IEnumerable<DocumentType> GetPositiveStockChangeDocumentTypes()
    {        
        yield return DocumentType.purchase_bill;
        yield return DocumentType.sales_return;
        yield return DocumentType.stock_adjustment_plus;
    }

    public static IEnumerable<DocumentType> GetNegativeStockChangeDocumentTypes()
    {     
        yield return DocumentType.sales_invoice;
        yield return DocumentType.purchase_return;
        yield return DocumentType.stock_adjustment_minus;
    }

}
