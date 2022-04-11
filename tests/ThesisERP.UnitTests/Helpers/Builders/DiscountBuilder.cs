using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;

namespace ThesisERP.UnitTests.Helpers.Builders;

public class DiscountBuilder : ITestDataBuilder<Discount>
{
    private Discount _Discount = new Discount();

    public DiscountBuilder WithId(int id)
    {
        _Discount.Id = id;
        return this;
    }

    public DiscountBuilder WithName(string name)
    {
        _Discount.Name = name;
        return this;
    }
    public DiscountBuilder WithDescription(string description)
    {
        _Discount.Description = description;
        return this;
    }

    public DiscountBuilder WithAmount(decimal amount)
    {
        _Discount.Amount = amount;
        return this;
    }

    public DiscountBuilder IsDeleted(bool isDeleted)
    {
        _Discount.IsDeleted = isDeleted;
        return this;
    }

    public DiscountBuilder WithDefaultValues()
    {
        _Discount = new Discount()
        {
            Id = 1,
            Name = "Test",
            Description = "Test Discount",
            Amount = 0.10m,
            IsDeleted = false
        };

        return this;
    }


    public Discount Build()
    {
        return _Discount;
    }

    public IEnumerable<Discount> BuildDefaultList(int length, bool withIds)
    {
        throw new System.NotImplementedException();
    }

    ITestDataBuilder<Discount> ITestDataBuilder<Discount>.WithDefaultValues()
    {
        return WithDefaultValues();
    }
}
