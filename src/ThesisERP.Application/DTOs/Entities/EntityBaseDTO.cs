using System.ComponentModel.DataAnnotations;

namespace ThesisERP.Application.DTOs.Entities;

public abstract class EntityBaseDTO
{
    public string? Organization { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    public AddressDTO BillingAddress { get; set; } = new AddressDTO();
    public AddressDTO ShippingAddress { get; set; } = new AddressDTO();
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class CreateEntityBaseDTO
{
    public string? Organization { get; set; }

    [Required]
    [StringLength(60, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    public AddressDTO BillingAddress { get; set; } = new AddressDTO();
    public AddressDTO ShippingAddress { get; set; } = new AddressDTO();
}

public abstract class UpdateEntityBaseDTO : CreateEntityBaseDTO { }

public abstract class EntityBasicInfoBaseDTO
{
    public int Id { get; set; }
    public string? Organization { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }

    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
}
