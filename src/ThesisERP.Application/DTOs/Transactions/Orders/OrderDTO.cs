using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions.Orders;

public class CreateOrderDTO : CreateTransactionBaseDTO<CreateOrderRowDTO>
{
}

public class UpdateOrderDTO : UpdateTransactionBaseDTO<CreateOrderRowDTO>
{
}
public class ProcessOrderDTO
{
    [Required]
    public int InventoryLocationId { get; set; }
}
public class FulfillOrderDTO
{
    [Required]
    public int FulfillmentDocumentTemplateId { get; set; }
    public bool CreateAsPending { get; set; } = false;
    public ICollection<FulfillOrderRowDTO>? FullfillmentRows { get; set; }
}

public class BaseOrderDTO : TransactionBaseDTO<OrderRowDTO>
{
    public OrderType Type { get; set; }
    public ICollection<int> RelatedDocumentIDs { get; set; }
    public bool IsPartiallyFulfilled { get; set; }
}

public class GenericOrderDTO : BaseOrderDTO
{
    public EntityBaseInfoDTO Entity { get; set; }
}