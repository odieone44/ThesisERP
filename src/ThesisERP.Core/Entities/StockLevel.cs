namespace ThesisERP.Core.Entities;

public class StockLevel
{
    public int Id { get; set; }
    public int InventoryLocationId { get; set; }
    public InventoryLocation InventoryLocation { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public decimal Available { get; set; } = decimal.Zero;
    public decimal Outgoing { get; set; } = decimal.Zero;
    public decimal Incoming { get; set; } = decimal.Zero;
    public decimal OnHand => Available - Outgoing + Incoming;

}
