using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions.Documents;

public class CreateDocumentDTO : CreateTransactionBaseDTO
{   
    [Required]
    public int InventoryLocationId { get; set; }  

    [Required]
    public ICollection<CreateDocumentRowDTO> Rows { get; set; } = new List<CreateDocumentRowDTO>();
}

public class UpdateDocumentDTO : UpdateTransactionBaseDTO
{
    public int? InventoryLocationId { get; set; }
    public ICollection<CreateDocumentRowDTO>? Rows { get; set; } = new List<CreateDocumentRowDTO>();
}

public class BaseDocumentDTO
{
    public int Id { get; set; }
    public DocumentType Type { get; set; }
    public string DocumentNumber { get; set; }
    public TransactionStatus Status { get; set; }

    [Required]
    public InventoryLocationBaseDTO InventoryLocation { get; set; }

    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;

    [Required]
    public int TemplateId { get; set; }

    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }

    [Required]
    public ICollection<DocumentRowDTO> Rows { get; set; } = new List<DocumentRowDTO>();
}

public class GenericDocumentDTO : BaseDocumentDTO
{
    public EntityBaseInfoDTO Entity { get; set; }
}

public class SalesDocumentDTO : BaseDocumentDTO
{
    public ClientBaseInfoDTO Client { get; set; }
}

public class PurchaseDocumentDTO : BaseDocumentDTO
{
    public SupplierBaseInfoDTO Supplier { get; set; }
}

public class EntityBaseInfoDTO
{
    public int Id { get; set; }
    public string? Organization { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}
