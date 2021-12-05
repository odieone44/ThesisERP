using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Core.Entities
{
    public class StockLevel
    {
        public int Id { get; set; }
        public int InventoryLocationId { get; set; }
        public InventoryLocation InventoryLocation { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Available { get; set; }
        public decimal Outgoing { get; set; }
        public decimal Incoming { get; set; }
        public decimal OnHand => Available - Outgoing + Incoming;
        
    }
}
