using Microsoft.EntityFrameworkCore;
using System;
using ThesisERP.Core.Entites;
using ThesisERP.Core.Enums;
using ThesisERP.Infrastracture.Data;

namespace ThesisERP
{
    public static class SeedDatabase
    {
        public static readonly Entity TestClient = new Entity(type: Entities.EntityTypes.client, 
                                                              firstName: "John",
                                                              lastName:"Doe", 
                                                              email:"example@test.com",
                                                              organization:"ThesisERP");

        public static readonly Entity TestSupplier = new Entity(type: Entities.EntityTypes.supplier,
                                                                firstName: "James",
                                                                lastName: "Harden",
                                                                email: "example2@test.com",
                                                                organization: "ThesisERP");

        public static readonly EntityAddress ClientShippingAddress = new EntityAddress
        {
            AddressType = Addresses.AddressTypes.shipping,
            FirstName = "Test",
            LastName = "Recipient",
            Email = "example@test.com",
            Organization = "exampleOrg",
            City = "Athens",
            Line1 = "Aristidou 8",
            PostalCode = "10505",
            Country = Addresses.CountryCodes.GR,
            Phone = "6890202333",
            TaxId = "00000000"
        };

        public static readonly EntityAddress ClientBillingAddress = new EntityAddress
        {
            AddressType = Addresses.AddressTypes.billiing,
            FirstName = "Test",
            LastName = "Billing",
            Email = "example@test.com",
            Organization = "exampleOrg",
            City = "Athens",
            Line1 = "Aristidou 8",
            PostalCode = "10505",
            Country = Addresses.CountryCodes.GR,
            Phone = "6890202333",
            TaxId = "00000000"
        };

        public static readonly EntityAddress SupplierShippingAddress = new EntityAddress
        {
            AddressType = Addresses.AddressTypes.shipping,
            FirstName = "Test",
            LastName = "Recipient",
            Email = "example@test.com",
            Organization = "exampleOrg",
            City = "Athens",
            Line1 = "Aristidou 8",
            PostalCode = "10505",
            Country = Addresses.CountryCodes.GR,
            Phone = "6890202333",
            TaxId = "00000000"
        };

        public static readonly EntityAddress SupplierBillingAddress = new EntityAddress
        {
            AddressType = Addresses.AddressTypes.billiing,
            FirstName = "Test",
            LastName = "Billing",
            Email = "example@test.com",
            Organization = "exampleOrg",
            City = "Athens",
            Line1 = "Aristidou 8",
            PostalCode = "10505",
            Country = Addresses.CountryCodes.GR,
            Phone = "6890202333",
            TaxId = "00000000"
        };

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

            TestClient.EntityAdresses.Add(ClientBillingAddress);
            TestClient.EntityAdresses.Add(ClientShippingAddress);

            dbContext.Entities.Add(TestClient);

            TestSupplier.EntityAdresses.Add(SupplierBillingAddress);
            TestSupplier.EntityAdresses.Add(SupplierShippingAddress);

            dbContext.Entities.Add(TestSupplier);

            dbContext.SaveChanges();
        }

    }
}
