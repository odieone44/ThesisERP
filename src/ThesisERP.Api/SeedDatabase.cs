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
                                                     firstName: "Jane",
                                                     lastName: "Doe",
                                                     email: "example2@test.com",
                                                     organization: "ThesisERP");

    public static readonly Address TestAddress = new(name: "Test Address Name",
                                                    line1: "Test Line 1",
                                                    line2: "Test Line 2",
                                                    city: "Athens",
                                                    region: "Attiki",
                                                    postalCode: "10505",
                                                    country: CountryCode.GR);

    public static readonly InventoryLocation TestInventoryLocation = new()
    {
        Name = "Test Location",
        Abbreviation = "TEST"
    };

    public static readonly DocumentTemplate TestSalesInvoice = new()
    {
        Name = "Sales Invoice",
        Abbreviation = "SI",
        Description = "Handles sales of goods to clients",
        NextNumber = 1,
        Prefix = "SI-",
        Postfix = string.Empty,
        DocumentType = DocumentType.sales_invoice,
        DateCreated = DateTime.UtcNow,
        DateUpdated = DateTime.UtcNow
    };

    public static readonly DocumentTemplate TestPurchaseBill = new()
    {
        Name = "Purchase Bill",
        Abbreviation = "PB",
        Description = "Handles purchase of goods from suppliers",
        NextNumber = 1,
        Prefix = "PB-",
        Postfix = string.Empty,
        DocumentType = DocumentType.purchase_bill,
        DateCreated = DateTime.UtcNow,
        DateUpdated = DateTime.UtcNow
    };

    public static readonly Product TestProduct = new()
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

    public async static Task Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();

        await PopulateTestDataAsync(dbContext);        

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var rolesManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedDefaultUserAsync(userManager, rolesManager, configuration);
    }

    public async static Task SeedDefaultUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
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

        var adminSettings = configuration.GetSection("DefaultAdminUser");
        var adminUsername = adminSettings.GetSection("username").Value;
        var adminPassword = adminSettings.GetSection("password").Value;

        var administrator = new AppUser { UserName = adminUsername, Email = adminUsername, FirstName = "admin", LastName = "test" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, adminPassword);
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

        TestClient.ShippingAddress = TestAddress.Copy();
        TestClient.BillingAddress = TestAddress.Copy();
        dbContext.Entities.Add(TestClient);

        TestSupplier.ShippingAddress = TestAddress.Copy();
        TestSupplier.BillingAddress = TestAddress.Copy();
        dbContext.Entities.Add(TestSupplier);
        
        await dbContext.SaveChangesAsync();

        TestInventoryLocation.Address = TestAddress.Copy();
        dbContext.InventoryLocations.Add(TestInventoryLocation);
        await dbContext.SaveChangesAsync();
                
        dbContext.DocumentTemplates.Add(TestSalesInvoice);
        dbContext.DocumentTemplates.Add(TestPurchaseBill);
        await dbContext.SaveChangesAsync();             

        dbContext.Products.Add(TestProduct);
        await dbContext.SaveChangesAsync();

        var stockEntry = new StockLevel()
        {
            InventoryLocation = TestInventoryLocation,
            Product = TestProduct,
            Available = 0.0m,
            Incoming = 0.0m,
            Outgoing = 0.0m
        };

        dbContext.StockLevels.Add(stockEntry);
        await dbContext.SaveChangesAsync();

    }

}
