using System;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.UnitTests;

public class EntityBuilder
{
    private Entity _entity = new Entity();

    public EntityBuilder WithId(int id)
    {
        _entity.Id = id;
        return this;
    }

    public EntityBuilder WithType(EntityType type)
    {
        _entity.EntityType = type;
        return this;
    }

    public EntityBuilder WithOrganization(string? organization)
    {
        _entity.Organization = organization;
        return this;
    }

    public EntityBuilder WithFirstName(string firstName)
    {
        _entity.FirstName = firstName;
        return this;
    }

    public EntityBuilder WithLastName(string? lastName)
    {
        _entity.LastName = lastName;
        return this;
    }

    public EntityBuilder WithEmail(string email)
    {
        _entity.Email = email;
        return this;
    }

    public EntityBuilder WithDateCreated(DateTime dateCreated)
    {
        _entity.DateCreated = dateCreated;
        return this;
    }

    public EntityBuilder WithDateUpdated(DateTime? dateUpdated)
    {
        _entity.DateUpdated = dateUpdated;
        return this;
    }

    public EntityBuilder IsDeleted(bool isDeleted)
    {
        _entity.IsDeleted = isDeleted;
        return this;
    }

    public EntityBuilder WithBillingAddress(Address address)
    {
        _entity.BillingAddress = address;
        return this;
    }

    public EntityBuilder WithShippingAddress(Address address)
    {
        _entity.ShippingAddress = address;
        return this;
    }

    public EntityBuilder WithClientTestValues()
    {

        Address testAddres = new(name: "Test Address Name",
                                line1: "Test Line 1",
                                line2: "Test Line 2",
                                city: "Athens",
                                region: "Attiki",
                                postalCode: "10505",
                                country: CountryCode.GR);

        _entity = new Entity()
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Client",
            Organization = "Test Organization",
            Email = "test@localhost",
            EntityType = EntityType.client,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null,
            IsDeleted = false,
            RelatedProducts = null,
            BillingAddress = testAddres.Copy(),
            ShippingAddress = testAddres.Copy()
        };

        return this;
    }

    public EntityBuilder WithSupplierTestValues()
    {

        Address testAddres = new(name: "Test Address Name",
                                line1: "Test Line 1",
                                line2: "Test Line 2",
                                city: "Athens",
                                region: "Attiki",
                                postalCode: "10505",
                                country: CountryCode.GR);

        _entity = new Entity()
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Supplier",
            Organization = "Test Organization",
            Email = "test@localhost",
            EntityType = EntityType.supplier,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null,
            IsDeleted = false,
            RelatedProducts = null,
            BillingAddress = testAddres.Copy(),
            ShippingAddress = testAddres.Copy()
        };

        return this;
    }

    public Entity Build()
    {
        return _entity;
    }


}
