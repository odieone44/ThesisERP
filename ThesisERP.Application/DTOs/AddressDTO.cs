using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs
{
    public class AddressDTO
    {
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [StringLength(300, ErrorMessage = "Address Line is limited to {1} characters")]
        public string Line1 { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [StringLength(300, ErrorMessage = "Address Line is limited to {1} characters")]
        public string Line2 { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "City field is limited to {1} characters")]
        public string City { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "Region field is limited to {1} characters")]
        public string Region { get; set; } = string.Empty;

        [DataType(DataType.PostalCode)]
        [StringLength(30, ErrorMessage = "Postal Code field is limited to {1} characters")]
        public string PostalCode { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Addresses.CountryCodes CountryCode { get; set; } = Addresses.CountryCodes.NONE;
    }
}
