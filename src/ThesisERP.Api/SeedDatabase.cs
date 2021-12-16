using Microsoft.AspNetCore.Identity;
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

    public async static Task Initialize(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();

        await PopulateTestDataAsync(dbContext);

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var rolesManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedDefaultUserAsync(userManager, rolesManager);
    }

    public async static Task SeedDefaultUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var administratorRole = new IdentityRole("Administrator");

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var userRole = new IdentityRole("User");

        if (roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await roleManager.CreateAsync(userRole);
        }

        var administrator = new AppUser { UserName = "administrator@localhost", Email = "administrator@localhost", FirstName = "admin", LastName = "test" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Te#stD@ta!1");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }
    }
    public async static Task PopulateTestDataAsync(DatabaseContext dbContext)
    {

        if (dbContext.Entities.Any() ||
           dbContext.Documents.Any() ||
           dbContext.Products.Any())
        {
            return;   // DB has been seeded
        }

        TestClient.ShippingAddress = ClientShippingAddress;
        TestClient.BillingAddress = ClientBillingAddress;

        dbContext.Entities.Add(TestClient);

        TestSupplier.ShippingAddress = SupplierShippingAddress;
        TestSupplier.BillingAddress = SupplierBillingAddress;

        dbContext.Entities.Add(TestSupplier);

        await dbContext.SaveChangesAsync();

        var location = new InventoryLocation()
        {
            Name = "Test Location",
            Abbreviation = "TEST",
            Address = ClientBillingAddress.Copy()
        };

        dbContext.InventoryLocations.Add(location);
        await dbContext.SaveChangesAsync();

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
        await dbContext.SaveChangesAsync();

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
        await dbContext.SaveChangesAsync();

        var stockEntry = new StockLevel()
        {
            InventoryLocation = location,
            Product = product,
            Available = 0.0m,
            Incoming = 0.0m,
            Outgoing = 0.0m
        };

        dbContext.StockLevels.Add(stockEntry);
        await dbContext.SaveChangesAsync();

    }

}
