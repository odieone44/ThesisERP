using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data
{
    [Owned]
    public class EntityAddress : IAddress
    {
        [Key]
        public int Id { get; set; }                
        public int EntityId { get; set; }
        public string Organization { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxId { get; set; }
        public Addresses.AddressType AddressType { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public Addresses.CountryCode Country { get; set; }
    }
}
