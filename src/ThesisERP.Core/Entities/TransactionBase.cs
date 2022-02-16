using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public abstract class TransactionBase
{
    public int Id { get; set; }
    public int EntityId { get; set; }
    public Entity Entity { get; set; }    
    public TransactionStatus Status { get; set; }
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
    public string Comments { get; set; } = string.Empty;        
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string CreatedBy { get; set; }    
    public bool IsFulfilled => Status == TransactionStatus.fulfilled;
    public bool IsClosed => Status == TransactionStatus.closed;
    public bool IsCancelled => Status == TransactionStatus.cancelled;

    public TransactionBase()
    {

    }
}
