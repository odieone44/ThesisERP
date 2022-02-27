using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public class OrderTemplate : TransactionTemplateBase
{
    public OrderType OrderType { get; set; }   
    public bool UsesClientEntity => OrderType == OrderType.sales_order;
    public bool UsesSupplierEntity => !UsesClientEntity;
}
