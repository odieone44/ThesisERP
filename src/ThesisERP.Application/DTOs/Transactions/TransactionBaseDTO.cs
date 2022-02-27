using System.ComponentModel.DataAnnotations;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions;

public abstract class CreateTransactionBaseDTO<T>
    where T : CreateTransactionRowBaseDTO
{
    [Required]
    public int EntityId { get; set; }
    [Required]
    public int TemplateId { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }
    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;
    [Required]
    public ICollection<T> Rows { get; set; } = new List<T>();
}

public abstract class UpdateTransactionBaseDTO<T>
    where T : CreateTransactionRowBaseDTO
{
    public int? EntityId { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }

    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;
    public ICollection<T>? Rows { get; set; }
}

public abstract class TransactionBaseDTO<T>
    where T : TransactionRowBaseDTO
{
    public int Id { get; set; }
    public string Number { get; set; }
    public TransactionStatus Status { get; set; }
    [StringLength(1000)]
    public string Comments { get; set; } = string.Empty;    
    public int TemplateId { get; set; }
    public AddressDTO BillingAddress { get; set; }
    public AddressDTO ShippingAddress { get; set; }    
    public ICollection<T> Rows { get; set; } = new List<T>();
}

public class EntityBaseInfoDTO
{
    public int Id { get; set; }
    public string? Organization { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}