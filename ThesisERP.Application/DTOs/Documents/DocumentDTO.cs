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
    public ICollection<CreateDocumentDetailDTO> Details { get; set; } = new List<CreateDocumentDetailDTO>();
}

public class UpdateDocumentDTO
{

}
