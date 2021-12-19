using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Documents;

public class CreateDocumentDTO
{
    [Required]
    public int EntityId { get; set; }

    [Required]
    public int InventoryLocationId { get; set; }

    [Required]
    public int TemplateId { get; set; }

    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }

    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;

    [Required]
    public ICollection<CreateDocumentRowDTO> Rows { get; set; } = new List<CreateDocumentRowDTO>();
}

public class UpdateDocumentDTO
{    
    public int? EntityId { get; set; }
        
    public int? InventoryLocationId { get; set; }
    
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }

    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;
        
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

public class DocumentDTO 
{    
    public int Id { get; set; }
    public DocumentType Type { get; set; }
    public string DocumentNumber { get; set; }
    public TransactionStatus Status { get; set; }
    public EntityBaseInfoDTO Entity { get; set; }

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
