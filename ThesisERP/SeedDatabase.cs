using Microsoft.EntityFrameworkCore;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;
using ThesisERP.Infrastracture.Data;

namespace ThesisERP;

public static class SeedDatabase
{
    public static readonly Entity TestClient = new(type: EntityType.client,
                                                   firstName: "John",
                                                   lastName: "Doe",
                                                   email: "example@test.com",
                                                   organization: "ThesisERP");

    public static readonly Entity TestSupplier = new(type: EntityType.supplier,
                                                     firstName: "James",
                                                     lastName: "Harden",
                                                     email: "example2@test.com",
                                                     organization: "ThesisERP");

    public static readonly Address ClientShippingAddress = new(name: "Test Recipient",
                                                               line1: "Aristidou 8",
                                                               line2: "4th Floor",
                                                               city: "Athens",
                                                               region: "Attiki",
                                                               postalCode: "10505",
                                                               country: CountryCode.GR);

    public static readonly Address ClientBillingAddress = new(name: "Test Billing",
                                                               line1: "Aristidou 8",
                                                               line2: "4th Floor",
                                                               city: "Athens",
                                                               region: "Attiki",
                                                               postalCode: "10505",
                                                               country: CountryCode.GR);

    public static readonly Address SupplierShippingAddress = new(name: "Test Recipient",
                                                                  line1: "Aristidou 8",
                                                                  line2: "4th Floor",
                                                                  city: "Athens",
                                                                  region: "Attiki",
                                                                  postalCode: "10505",
                                                                  country: CountryCode.GR);

    public static readonly Address SupplierBillingAddress = new(name: "Test Billing",
                                                                 line1: "Aristidou 8",
                                                                 line2: "4th Floor",
                                                                 city: "Athens",
                                                                 region: "Attiki",
                                                                 postalCode: "10505",
                                                                 country: CountryCode.GR);


    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new DatabaseContext(serviceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>()))
        {

            if (dbContext.Entities.Any())
            {
                return;   // DB has been seeded
            }

            PopulateTestData(dbContext);
        }
    }

    public static void PopulateTestData(DatabaseContext dbContext)
    {
        foreach (var item in dbContext.Entities)
        {
            dbContext.Remove(item);
        }

        dbContext.SaveChanges();

        TestClient.ShippingAddress = ClientShippingAddress;
        TestClient.BillingAddress = ClientBillingAddress;

        dbContext.Entities.Add(TestClient);

        TestSupplier.ShippingAddress = SupplierShippingAddress;
        TestSupplier.BillingAddress = SupplierBillingAddress;

        dbContext.Entities.Add(TestSupplier);

        dbContext.SaveChanges();

        var location = new InventoryLocation()
        {
            Name = "Test Location",
            Abbreviation = "TEST",
            Address = ClientBillingAddress.Copy()
        };

        dbContext.InventoryLocations.Add(location);
        dbContext.SaveChanges();

        var template = new DocumentTemplate()
        {
            Name = "Sales Invoice",
            Abbreviation = "SI",
            Description = "Handles sales of goods to clients",
            NextNumber = 1,
            Prefix = "SI-",
            Postfix = string.Empty,
            DocumentType = DocumentType.sales_invoice,
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now
        };

        dbContext.DocumentTemplates.Add(template);
        dbContext.SaveChanges();

        var product = new Product()
        {
            SKU = "TST0001",
            Description = "Test Product",
            LongDescription = "This is a test product.",
            Type = ProductType.product,
            DefaultPurchasePrice = 10.0m,
            DefaultSaleSPrice = 20.0m,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        dbContext.Products.Add(product);
        dbContext.SaveChanges();

        var stockEntry = new StockLevel()
        {
            InventoryLocation = location,
            Product = product,
            Available = 0.0m,
            Incoming = 0.0m,
            Outgoing = 0.0m
        };

        dbContext.StockLevels.Add(stockEntry);
        dbContext.SaveChanges();

    }

}
