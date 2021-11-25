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
    [Table("DocumentAddresses")]
    public class DocumentAddress : IAddress
    {
        [Key]
        public int Id { get; set; }
        public Addresses.AddressTypes AddressType { get; set; }
        
        [ForeignKey(nameof(Document))]
        public int DocumentId { get; set; }
        public Document Document { get; set; }

        public string Organization { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TaxId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public Addresses.CountryCodes Country { get; set; }
        

    
    }
}
