namespace ThesisERP.Application.DTOs;

public class StockLevelDTO : StockInfo
{
    public int InventoryLocationId { get; set; }
    public InventoryLocationDTO InventoryLocation { get; set; }
    public int ProductId { get; set; }
    public ProductDTO Product { get; set; }
}


public class GetLocationStockDTO
{
    public int InventoryLocationId { get; set; }
    public string InventoryLocationName { get; set; }
    public string InventoryLocationAbbreviation { get; set; }
    public ICollection<ProductStockLevelDTO> ProductStockLevels { get; set; } = new List<ProductStockLevelDTO>();

}


public class GetProductStockDTO
{
    public int ProductId { get; set; }
    public string ProductSKU { get; set; }
    public string ProductDescription { get; set; }
    public ICollection<LocationStockLevelDTO> LocationStockLevels { get; set; } = new List<LocationStockLevelDTO>();
}

public class ProductStockLevelDTO : StockInfo
{
    public int ProductId { get; set; }
    public string ProductSKU { get; set; }
    public string ProductDescription { get; set; }
}

public class LocationStockLevelDTO : StockInfo
{
    public int InventoryLocationId { get; set; }
    public string InventoryLocationName { get; set; }
    public string InventoryLocationAbbreviation { get; set; }
}

public abstract class StockInfo
{
    public decimal Available { get; set; }
    public decimal Outgoing { get; set; }
    public decimal Incoming { get; set; }
    public decimal OnHand => Available - Outgoing + Incoming;
}
