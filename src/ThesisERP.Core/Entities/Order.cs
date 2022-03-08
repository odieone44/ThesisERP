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

    private Order() : base() { }

    private Order(
        Entity entity,
        OrderTemplate template,
        Address billingAddress,
        Address shippingAddress,
        string username) 
        : base(entity, template, billingAddress, shippingAddress, username) 
    {
    }

    public static Order Initialize(
        Entity entity,
        OrderTemplate template,
        Address billingAddress,
        Address shippingAddress,
        string username)
    {
        return new Order(entity, template, billingAddress, shippingAddress, username);
    }
}
