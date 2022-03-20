using System;
using System.Collections.Generic;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.UnitTests.Helpers.Builders;

internal class AddressBuilder : ITestDataBuilder<Address>
{
    private Address _address = new Address();

    public AddressBuilder WithName(string name)
    {
        _address.Name = name;
        return this;
    }

    public AddressBuilder WithLine1(string line1)
    {
        _address.Line1 = line1;
        return this;
    }

    public AddressBuilder WithLine2(string line2)
    {
        _address.Line2 = line2;
        return this;
    }

    public AddressBuilder WithCity(string city)
    {
        _address.City = city;
        return this;
    }

    public AddressBuilder WithRegion(string region)
    {
        _address.Region = region;
        return this;
    }

    public AddressBuilder WithPostalCode(string postalCode)
    {
        _address.PostalCode = postalCode;
        return this;
    }

    public AddressBuilder WithCountryCode(CountryCode country)
    {
        _address.Country = country;
        return this;
    }

    public AddressBuilder WithDefaultValues()
    {
        _address = new Address(name: "Test Address Name",
                                line1: "Test Line 1",
                                line2: "Test Line 2",
                                city: "Athens",
                                region: "Attiki",
                                postalCode: "10505",
                                country: CountryCode.GR);

        return this;
    }

    public Address Build()
    {
        return _address;
    }

    public static IEnumerable<Address> BuildDefaultList(int length)
    {
        if (length < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "length has to be positive");
        }

        for (var i = 0; i < length; i++)
        {
            var address = new AddressBuilder()
                                .WithDefaultValues()
                                .WithName($"Test Address {i + 1} Name")
                                .Build();


            yield return address;
        }
    }

    IEnumerable<Address> ITestDataBuilder<Address>.BuildDefaultList(int length, bool withIds)
    {
        return BuildDefaultList(length);
    }

    ITestDataBuilder<Address> ITestDataBuilder<Address>.WithDefaultValues()
    {
        return WithDefaultValues();
    }
}
