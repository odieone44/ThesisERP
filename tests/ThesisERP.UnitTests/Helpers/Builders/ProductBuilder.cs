using System;
using System.Collections.Generic;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.UnitTests.Helpers.Builders;

public class ProductBuilder : ITestDataBuilder<Product>
{
    private Product _product = new();

    public ProductBuilder WithId(int id)
    {
        _product.Id = id;
        return this;
    }

    public ProductBuilder WithType(ProductType type)
    {
        _product.Type = type;
        return this;
    }

    public ProductBuilder WithSku(string sku)
    {
        _product.SKU = sku;
        return this;
    }

    public ProductBuilder WithDescription(string description)
    {
        _product.Description = description;
        return this;
    }

    public ProductBuilder WithLongDescription(string? longDescription)
    {
        _product.LongDescription = longDescription;
        return this;
    }

    public ProductBuilder WithDefaultPurchasePrice(decimal? purchasePrice)
    {
        _product.DefaultPurchasePrice = purchasePrice;
        return this;
    }

    public ProductBuilder WithDefaultSalesPrice(decimal? salesPrice)
    {
        _product.DefaultSaleSPrice = salesPrice;
        return this;
    }

    public ProductBuilder WithDateCreated(DateTime dateCreated)
    {
        _product.DateCreated = dateCreated;
        return this;
    }

    public ProductBuilder WithDateUpdated(DateTime dateUpdated)
    {
        _product.DateUpdated = dateUpdated;
        return this;
    }

    public ProductBuilder WithRelatedEntities(List<Entity> entities)
    {
        _product.RelatedEntities = entities;
        return this;
    }

    public ProductBuilder WithStockLevels(List<StockLevel> stockLevels)
    {
        _product.StockLevels = stockLevels;
        return this;
    }

    public ProductBuilder IsDeleted(bool isDeleted)
    {
        _product.IsDeleted = isDeleted;
        return this;
    }

    public ProductBuilder WithDefaultValues()
    {
        _product = new Product()
        {
            Id = 1,
            Type = ProductType.product,
            SKU = "TEST-001",
            Description = "Test Product 001",
            LongDescription = "This is the first test product.",
            DefaultPurchasePrice = 1.0m,
            DefaultSaleSPrice = 10.0m,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null,
            IsDeleted = false
        };

        return this;
    }

    public static IEnumerable<Product> BuildDefaultList(int length, bool withIds)
    {
        if (length < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "length has to be positive");
        }

        for (var i = 0; i < length; i++)
        {
            var productBuilder = new ProductBuilder()
                                    .WithDefaultValues()
                                    .WithSku($"TEST-00{i + 1}")
                                    .WithDescription($"Test Product {i + 1}")
                                    .WithLongDescription($"This is test product number {i + 1}");
                                    
            if (withIds)
            {
                productBuilder.WithId(i+1);
            }

            yield return productBuilder.Build();
        }

    }

    public Product Build()
    {
        return _product;
    }

    IEnumerable<Product> ITestDataBuilder<Product>.BuildDefaultList(int length, bool withIds)
    {
        return BuildDefaultList(length, withIds);
    }

    ITestDataBuilder<Product> ITestDataBuilder<Product>.WithDefaultValues()
    {
        return WithDefaultValues();
    }
}
