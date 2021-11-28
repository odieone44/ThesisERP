using ThesisERP.Static.Enums;

namespace ThesisERP.Core.Interfaces
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
