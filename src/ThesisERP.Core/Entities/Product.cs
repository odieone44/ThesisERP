using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public ProductType Type { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public string? LongDescription { get; set; }
    public decimal? DefaultPurchasePrice { get; set; }
    public decimal? DefaultSaleSPrice { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
    public byte[] Timestamp { get; set; }
    public virtual ICollection<Entity> RelatedEntities { get; set; } = new List<Entity>();
    public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();

}
