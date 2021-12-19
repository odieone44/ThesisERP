using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Core.Entities;

public class InventoryLocation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public Address Address { get; set; }

    public bool IsDeleted { get; set; } = false;
    public ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
}
