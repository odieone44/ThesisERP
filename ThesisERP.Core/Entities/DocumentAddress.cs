using ThesisERP.Core.Interfaces;
using ThesisERP.Static.Enums;

namespace ThesisERP.Core.Entites
{
    public class DocumentAddress : IAddress
    {
        public int Id { get; set; }
        public Addresses.AddressTypes AddressType { get; set; }

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
