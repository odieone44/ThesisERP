using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;
using ThesisERP.Core.Models;

namespace ThesisERP.Core.Entities
{
    public class Address : ValueObject
    {
        public string Name { get; private set; } = string.Empty;                 
        public string Line1 { get; private set; } = string.Empty;
        public string Line2 { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Region { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public Addresses.CountryCodes Country { get; private set; } = Addresses.CountryCodes.NONE;
        
        private Address() { }

        public Address(string name,                        
                       string line1, 
                       string line2, 
                       string city, 
                       string region, 
                       string postalCode, 
                       Addresses.CountryCodes country)
        {
            Name = name;           
            Line1 = line1;
            Line2 = line2;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
        }

        public Address Copy()
        {
            return new(Name, Line1, Line2, City, Region, PostalCode, Country);
        }
        
        public override string ToString()
        {
            return $"{Name}, {Line1} {Line2}, {City}, {Region} {PostalCode}, {Country}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {            
            yield return Name;            
            yield return Line1;
            yield return Line2;
            yield return City;
            yield return Region;
            yield return PostalCode;
            yield return Country;
        }
    }
}
