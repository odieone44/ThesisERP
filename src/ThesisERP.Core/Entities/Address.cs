using ThesisERP.Core.Enums;
using ThesisERP.Core.Models;

namespace ThesisERP.Core.Entities;

public class Address : ValueObject
{
    public string Name { get; set; } = string.Empty;
    public string Line1 { get; set; } = string.Empty;
    public string Line2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public CountryCode Country { get; set; } = CountryCode.NONE;

    public Address() { }

    public Address(string name,
                   string line1,
                   string line2,
                   string city,
                   string region,
                   string postalCode,
                   CountryCode country)
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
