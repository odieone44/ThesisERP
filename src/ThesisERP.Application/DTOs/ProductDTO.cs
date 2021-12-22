using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs;

public class ProductDTO : CreateProductDTO
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
        
    public virtual ICollection<ClientDTO> RelatedClients { get; set; } = new List<ClientDTO>();    
    public virtual ICollection<SupplierDTO> RelatedSuppliers { get; set; } = new List<SupplierDTO>();

    //todo: not implemented yet
    //public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>(); 
}

public class CreateProductDTO
{    
    public ProductType Type { get; set; }
       
    [Required]    
    [StringLength(40, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string SKU { get; set; } = string.Empty;

    [Required]
    [StringLength(200, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [StringLength(4000, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 0)]
    public string? LongDescription { get; set; }
    
    [DataType(DataType.Currency)]
    public decimal? DefaultPurchasePrice { get; set; } = decimal.Zero;
    [DataType(DataType.Currency)]
    public decimal? DefaultSaleSPrice { get; set; } = decimal.Zero;        
    
}

public class UpdateProductDTO : CreateProductDTO
{
}