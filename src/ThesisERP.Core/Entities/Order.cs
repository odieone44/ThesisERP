using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Order : TransactionBase
{   
    public int TemplateId { get; set; }
    public OrderTemplate OrderTemplate { get; set; }
    public string OrderNumber { get; set; }
    public ICollection<OrderRow> Rows { get; set; } = new List<OrderRow>();
    public byte[] Timestamp { get; set; }   
    public OrderType Type => OrderTemplate.OrderType;    
    public bool IsPartiallyFulfilled => Rows.Count < Rows.Where(row => row.RowIsFulfilled).Count() && Rows.Where(row => row.RowIsFulfilled).Any();
}
