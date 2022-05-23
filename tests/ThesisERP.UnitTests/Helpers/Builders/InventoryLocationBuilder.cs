using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.UnitTests.Helpers.Builders;

public class InventoryLocationBuilder : ITestDataBuilder<InventoryLocation>
{
    private InventoryLocation _location = new();

    public InventoryLocationBuilder WithId(int id)
    {
        _location.Id = id;
        return this;
    }

    public InventoryLocationBuilder WithName(string name)
    {
        _location.Name = name;
        return this;
    }

    public InventoryLocationBuilder WithAbbreviation(string abbreviation) 
    { 
        _location.Abbreviation = abbreviation;
        return this;
    }

    public InventoryLocationBuilder WithAddress(Address address)
    {
        _location.Address = address;
        return this;
    }

    public InventoryLocationBuilder IsDeleted(bool isDeleted)
    {
        _location.IsDeleted = isDeleted;
        return this;
    }

    public InventoryLocationBuilder WithStockLevels(List<StockLevel> stockLevels)
    {
        _location.StockLevels = stockLevels;
        return this;
    }

    public InventoryLocationBuilder WithDefaultValues()
    {
        _location = new InventoryLocation()
        {
            Id = 1,
            Name = "Test Location",
            Abbreviation = "TEST",
            Address = new AddressBuilder().WithDefaultValues().Build(),
            IsDeleted = false,
            StockLevels = new List<StockLevel>()
        };

        return this;
    }

    public InventoryLocation Build()
    {
        return _location;
    }

    public IEnumerable<InventoryLocation> BuildDefaultList(int length, bool withIds)
    {
        throw new NotImplementedException();
    }

    ITestDataBuilder<InventoryLocation> ITestDataBuilder<InventoryLocation>.WithDefaultValues()
    {
        return WithDefaultValues();
    }
}
