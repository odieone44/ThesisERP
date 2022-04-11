using System.Collections.Generic;
using ThesisERP.Core.Entities;

namespace ThesisERP.UnitTests.Helpers.Builders;

public class TaxBuilder : ITestDataBuilder<Tax>
{

    private Tax _tax = new Tax();

    public TaxBuilder WithId(int id)
    {
        _tax.Id = id;
        return this;
    }

    public TaxBuilder WithName(string name)
    {
        _tax.Name = name;
        return this;
    }
    public TaxBuilder WithDescription(string description)
    {
        _tax.Description = description;
        return this;
    }

    public TaxBuilder WithAmount(decimal amount)
    {
        _tax.Amount = amount;
        return this;
    }

    public TaxBuilder IsDeleted(bool isDeleted)
    {
        _tax.IsDeleted = isDeleted;
        return this;
    }

    public TaxBuilder WithDefaultValues()
    {
        _tax = new Tax()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Tax",
            Amount = 0.24m,
            IsDeleted = false
        };

        return this;
    }


    public Tax Build()
    {
        return _tax;
    }

    public IEnumerable<Tax> BuildDefaultList(int length, bool withIds)
    {
        throw new System.NotImplementedException();
    }

    ITestDataBuilder<Tax> ITestDataBuilder<Tax>.WithDefaultValues()
    {
        return WithDefaultValues();
    }
}
