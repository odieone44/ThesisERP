using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.DTOs;

public class InventoryLocationDTO : CreateInventoryLocationDTO
{
    public int Id { get; set; }   
}

public class CreateInventoryLocationDTO
{   
    [Required]
    [StringLength(20, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(5, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Abbreviation { get; set; } = string.Empty;
    
    public AddressDTO Address { get; set; } = new AddressDTO();
}

public class UpdateInventoryLocationDTO : CreateInventoryLocationDTO
{
}