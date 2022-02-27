using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class Order : TransactionBase<OrderTemplate,OrderRow>
{       
    public ICollection<Document> RelatedDocuments { get; set; } = new List<Document>();      
    public OrderType Type => Template.OrderType;    
    public bool IsPartiallyFulfilled => Rows.Count > Rows.Where(row => row.RowIsFulfilled).Count() && Rows.Where(row => row.RowIsFulfilled).Any();
}
