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
    
    public interface IAddress
    {   
        public int Id { get; set; }
        public Addresses.AddressTypes AddressType { get; set; }                
        public string Line1 { get; set; } 
        public string Line2 { get; set; } 
        public string City { get; set; } 
        public string Region { get; set; }
        public string PostalCode { get; set; } 
        public Addresses.CountryCodes Country { get; set; }        
        
    }
}
