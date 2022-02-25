using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs.Transactions;

public abstract class CreateTransactionBaseDTO
{
    [Required]
    public int EntityId { get; set; }
    [Required]
    public int TemplateId { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }
    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;
}

public abstract class UpdateTransactionBaseDTO
{
    public int? EntityId { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }

    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;
}

public abstract class TransactionBaseDTO
{

}
