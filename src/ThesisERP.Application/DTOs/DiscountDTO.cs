using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs;

public class DiscountDTO : CreateDiscountDTO
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}

public class UpdateDiscountDTO
{
    [Required]
    [StringLength(20, MinimumLength = 1)]
    public string Name { get; set; }
    
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
 
}

public class CreateDiscountDTO : UpdateDiscountDTO
{

    [Required]
    [Range(0.0, 1.0)]
    public decimal Amount { get; set; }
}
