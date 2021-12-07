﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ThesisERP.Application.DTOs;

public class ClientDTO : GetClientDTO
{
    public virtual ICollection<ProductDTO>? RelatedProducts { get; set; } = new List<ProductDTO>();
}

public class GetClientDTO : CreateClientDTO
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
}

public class CreateClientDTO
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

public class UpdateClientDTO : CreateClientDTO
{
}
