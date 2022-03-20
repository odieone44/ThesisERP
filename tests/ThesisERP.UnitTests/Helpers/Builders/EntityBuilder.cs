using System;
using System.Collections.Generic;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.UnitTests.Helpers.Builders;

public class EntityBuilder : ITestDataBuilder<Entity>
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

    public EntityBuilder WithDefaultValues()
    {
        var testAddress = new AddressBuilder().WithDefaultValues().Build();

        _entity = new Entity()
        {
            Id = 1,
            FirstName = "Test",
            LastName = "Entity",
            Organization = "Test Organization",
            Email = "test@localhost",            
            DateCreated = DateTime.UtcNow,
            DateUpdated = null,
            IsDeleted = false,
            RelatedProducts = null,
            BillingAddress = testAddress.Copy(),
            ShippingAddress = testAddress.Copy()
        };

        return this;
    }

    public EntityBuilder WithDefaultClientValues()
    {
        return WithDefaultValues()
                .WithType(EntityType.client)
                .WithLastName("Client");
    }

    public EntityBuilder WithDefaultSupplierValues()
    {
        return WithDefaultValues()
                .WithType(EntityType.supplier)
                .WithLastName("Supplier");
    }

    public Entity Build()
    {
        return _entity;
    }

    /// <summary>
    /// Get a list of test entities with the provided length.
    /// </summary>
    /// <param name="length">The length of the list</param>
    /// <param name="withIds">True if entities should be given Ids</param>
    /// <param name="entityType">Will only return entities with given type if provided, otherwise the list will contain both types.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<Entity> BuildDefaultList(int length, bool withIds, EntityType? entityType = null)
    {
        if (length < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "length has to be positive");
        }

        for (var i = 0; i < length; i++)
        {
            EntityType type;

            if (entityType is null)
            {
                type = i % 2 == 0 ? EntityType.client : EntityType.supplier;
            }
            else
            {
                type = (EntityType)entityType;
            }

            var entityBuilder = new EntityBuilder()
                                .WithDefaultValues()
                                .WithType(type)
                                .WithLastName($"Entity {i + 1}");            

            if (withIds)
            {
                entityBuilder.WithId(i+1);
            }

            yield return entityBuilder.Build();
        }
    }

    ITestDataBuilder<Entity> ITestDataBuilder<Entity>.WithDefaultValues()
    {
        return WithDefaultValues();
    }

    IEnumerable<Entity> ITestDataBuilder<Entity>.BuildDefaultList(int length, bool withIds)
    {
        return BuildDefaultList(length, withIds, null);
    }
}
