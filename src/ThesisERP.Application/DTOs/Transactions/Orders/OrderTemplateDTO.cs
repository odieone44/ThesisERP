using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions.Orders;

public class OrderTemplateDTO : TransactionTemplateBaseDTO
{
    public OrderType OrderType { get; set; }
}

public class CreateOrderTemplateDTO : CreateTransactionTemplateBaseDTO
{
    [Required]
    public OrderType OrderType { get; set; }
}

public class UpdateOrderTemplateDTO : UpdateTransactionTemplateBaseDTO
{
}
