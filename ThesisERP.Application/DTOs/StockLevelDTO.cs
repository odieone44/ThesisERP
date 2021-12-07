using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.DTOs;

public class StockLevelDTO
{
    public int Id { get; set; }
    public int InventoryLocationId { get; set; }
    public InventoryLocationDTO InventoryLocation { get; set; }
    public int ProductId { get; set; }
    public ProductDTO Product { get; set; }
    public decimal Available { get; set; }
    public decimal Outgoing { get; set; }
    public decimal Incoming { get; set; }
    public decimal OnHand => Available - Outgoing + Incoming;
}


public class LocationStockDTO
{
    public int InventoryLocationId { get; set; }
    public string InventoryLocationName { get; set; }
    public string InventoryLocationAbbreviation { get; set; }
    public ICollection<ProductStockLevelDTO> ProductStockLevels { get; set; } = new List<ProductStockLevelDTO>();

}

public class ProductStockLevelDTO
{
    public int ProductId { get; set; }   
    public string ProductSKU { get; set; }
    public string ProductDescription { get; set; }
    public decimal Available { get; set; }
    public decimal Outgoing { get; set; }
    public decimal Incoming { get; set; }
    public decimal OnHand => Available - Outgoing + Incoming;
}
