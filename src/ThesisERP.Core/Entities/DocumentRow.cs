using ThesisERP.Core.Extensions;

namespace ThesisERP.Core.Entities;

#pragma warning disable CS8618

public class DocumentRow : TransactionRowBase
{
    public int ParentDocumentId { get; set; }
    public Document ParentDocument { get; set; }

    public byte[] Timestamp { get; private set; }

    public DocumentRow(int lineNumber,Product product, decimal quantity, decimal price, Tax? tax = null, Discount? discount = null) 
        : base(lineNumber, product, quantity, price, tax, discount)
    {
    }
    private DocumentRow()
    {

    }

}
#pragma warning restore CS8618

