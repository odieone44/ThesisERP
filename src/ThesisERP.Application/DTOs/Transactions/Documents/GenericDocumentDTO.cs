using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions.Documents;

public class CreateDocumentDTO : CreateTransactionBaseDTO<CreateDocumentRowDTO>
{   
    [Required]
    public int InventoryLocationId { get; set; }
    public bool CreateAsFulfilled { get; set; } = false;

}

public class UpdateDocumentDTO : UpdateTransactionBaseDTO<CreateDocumentRowDTO>
{
    public int? InventoryLocationId { get; set; }
}

public class BaseDocumentDTO : TransactionBaseDTO<DocumentRowDTO>
{    
    public DocumentType Type { get; set; }        
    public InventoryLocationBaseDTO InventoryLocation { get; set; }
}

public class GenericDocumentDTO : BaseDocumentDTO
{
    public EntityBaseInfoDTO Entity { get; set; }
}

//public class SalesDocumentDTO : BaseDocumentDTO
//{
//    public ClientBaseInfoDTO Client { get; set; }
//}

//public class PurchaseDocumentDTO : BaseDocumentDTO
//{
//    public SupplierBaseInfoDTO Supplier { get; set; }
//}
