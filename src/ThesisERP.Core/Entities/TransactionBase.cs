using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public abstract class TransactionBase<TTemplate, TRow>
    where TTemplate : TransactionTemplateBase
    where TRow : TransactionRowBase
{
    public int Id { get; set; }
    public string Number { get; set; }
    public int TemplateId { get; set; }
    public TTemplate Template { get; set; }
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
    public ICollection<TRow> Rows { get; set; } = new List<TRow>();
    public byte[] Timestamp { get; set; }

    public TransactionBase()
    {

    }

    public TransactionBase(Entity entity,
                           TTemplate template,
                           Address billingAddress,
                           Address shippingAddress,                                      
                           string username)
    {
        Entity = entity;
        EntityId = entity.Id;
        Template = template;
        TemplateId = template.Id;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        Number = $"{template.Prefix}{template.NextNumber}{template.Postfix}";
        Status = TransactionStatus.draft;
        DateCreated = DateTime.UtcNow;
        DateUpdated = DateTime.UtcNow;
        CreatedBy = username;
    }
}
