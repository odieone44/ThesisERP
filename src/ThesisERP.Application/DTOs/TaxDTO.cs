using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs;

public class TaxDTO : CreateTaxDTO
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class UpdateTaxDTO
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string Name { get; set; }
    
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    
}

public class CreateTaxDTO : UpdateTaxDTO
{    
    [Required]
    [Range(0.0, 1.0)]
    public decimal Amount { get; set; }
}
