﻿using ThesisERP.Core.Interfaces;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entites
{
    public class EntityAddress
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string? Organization { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? TaxId { get; set; }
        public Addresses.AddressTypes AddressType { get; set; }
        public string Line1 { get; set; } = string.Empty;
        public string? Line2 { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public Addresses.CountryCodes Country { get; set; }
        public Entity Entity { get; set; }
    }
}
