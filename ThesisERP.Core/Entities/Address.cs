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
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;                     
        public string Line1 { get; private set; } = string.Empty;
        public string Line2 { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Region { get; private set; } = string.Empty;
        public string PostalCode { get; private set; } = string.Empty;
        public Addresses.CountryCodes Country { get; private set; }
        
        private Address() { }

        public Address(string firstName, 
                       string lastName, 
                       string line1, 
                       string line2, 
                       string city, 
                       string region, 
                       string postalCode, 
                       Addresses.CountryCodes country)
        {
            FirstName = firstName;
            LastName = lastName;
            Line1 = line1;
            Line2 = line2;
            City = city;
            Region = region;
            PostalCode = postalCode;
            Country = country;
        }
        
        public override string ToString()
        {
            return $"{FirstName} {LastName}, {Line1} {Line2}, {City}, {Region} {PostalCode}, {Country}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            //yield return Organization;
            yield return FirstName;
            yield return LastName;
            //yield return Email;
            //yield return Phone;
            //yield return TaxId;
            yield return Line1;
            yield return Line2;
            yield return City;
            yield return Region;
            yield return PostalCode;
            yield return Country;
        }
    }
}
