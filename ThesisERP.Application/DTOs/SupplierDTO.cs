using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.DTOs
{
    public class SupplierDTO : CreateSupplierDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Product>? RelatedProducts { get; set; }
    }

    public class CreateSupplierDTO
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

    public class UpdateSupplierDTO : CreateSupplierDTO { }
}
