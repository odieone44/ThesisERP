using Moq;
using Xunit;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;

using ThesisERP.Core.Enums;
using ThesisERP.Core.Models;
using ThesisERP.Core.Entities;
using ThesisERP.UnitTests.Helpers;
using ThesisERP.Application.Mappings;
using ThesisERP.Application.Interfaces;
using ThesisERP.UnitTests.Helpers.Builders;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Application.DTOs.Transactions.Documents;

namespace ThesisERP.UnitTests.Application.Services.DocumentService_Tests;

public class DocumentService_CreateAsync
{
    private Mock<IApiService> _mockApi = new();
    private Mock<IStockService> _mockStock = new();

    private DocumentService _documentService;
    private IMapper _mapper;

    public DocumentService_CreateAsync()
    {
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MapperInitializer>();
        });

        _mapper = mockMapper.CreateMapper();

        _documentService = new DocumentService(_mockApi.Object, _mockStock.Object, _mapper);
    }

    [Fact]
    public async Task ShouldCreatePendingDocument_WhenAllEntitiesExist()
    {
        var testLocation = new InventoryLocationBuilder().WithDefaultValues().Build();
        var testTemplate = new DocumentTemplateBuilder().WithDefaultPurchaseBillValues().Build();
        var testSuppler = new EntityBuilder().WithDefaultSupplierValues().Build();
        var testProduct = new ProductBuilder().WithDefaultValues().Build();
        var testDiscount = new DiscountBuilder().WithDefaultValues().Build();
        var testTax = new TaxBuilder().WithDefaultValues().Build();
                
        _SetupMocksForDocumentCreate(
            testProducts: new List<Product> { testProduct },
            testLocation: testLocation,
            testEntity: testSuppler,
            testTaxes: new List<Tax> { testTax },
            testDiscounts: new List<Discount> { testDiscount },
            testTemplate: testTemplate);

        var newDocument = new CreateDocumentDTO()
        {
            BillingAddress = new ThesisERP.Application.DTOs.AddressDTO()
            {
                City = "Test",
                CountryCode = CountryCode.GR
            },
            ShippingAddress = new ThesisERP.Application.DTOs.AddressDTO()
            {
                City = "Test",
                CountryCode = CountryCode.GR
            },
            Comments = "This is a test purchase bill.",
            EntityId = testSuppler.Id,
            InventoryLocationId = testLocation.Id,
            Rows = new List<CreateDocumentRowDTO>() { new CreateDocumentRowDTO()
            {
                ProductId = testProduct.Id,
                ProductQuantity = 1,
                UnitPrice = testProduct.DefaultPurchasePrice ?? 1.0m,
                DiscountID = testTax.Id,
                TaxID = testDiscount.Id
            } },
            TemplateId = testTemplate.Id
        };

        var result = await _documentService.CreateAsync(newDocument, "unit_test");

        Assert.NotNull(result);
        Assert.Equal(testTemplate.Id, result.TemplateId);
        Assert.Equal(newDocument.Rows.Count, result.Rows.Count);
        Assert.Equal(testTemplate.DocumentType, result.Type);
        Assert.Equal(TransactionStatus.pending, result.Status);
        Assert.Equal(2, testTemplate.NextNumber);
        Assert.True(result.Id > 0);
    }

    private void _SetupMocksForDocumentCreate(
        List<Product>? testProducts,
        InventoryLocation? testLocation,
        Entity? testEntity,
        List<Tax>? testTaxes,
        List<Discount>? testDiscounts,
        DocumentTemplate? testTemplate)
    {

        var docsRepo = new MockRepositoryBase<Document>()
                            .MockUpdate()
                            .MockSaveChanges()
                            .MockAdd((Document x) =>
                            {
                                x.Id = 1;
                                return x;
                            });

        //var testDocs = new List<Document>();
        //var ordersRepo = new Mock<IRepositoryBase<Order>>();
        //var testOrders = new List<Order>();

        var productsRepo = new MockRepositoryBase<Product>();
        if (testProducts != null)
        {
            productsRepo.MockGetAll(testProducts);
        }

        var documentTemplatesRepo = new MockRepositoryBase<DocumentTemplate>();
        if (testTemplate is not null)
        {
            documentTemplatesRepo.MockGetById(testTemplate);
        }
        documentTemplatesRepo.MockUpdate();

        var entitiesRepo = new MockRepositoryBase<Entity>();
        if (testEntity is not null)
        {
            var entities = new List<Entity>() { testEntity };
            entitiesRepo.MockGetById(testEntity);
            entitiesRepo.MockGetAll(entities);
        }
        else
        {
            entitiesRepo.MockGetAll(new List<Entity>());
        }

        var locationsRepo = new MockRepositoryBase<InventoryLocation>();
        if (testLocation is not null)
        {
            locationsRepo.MockGetById(testLocation);
        }

        //var stockRepo = new Mock<IRepositoryBase<StockLevel>>();
        //var testStock = new List<StockLevel>();

        var taxesRepo = new MockRepositoryBase<Tax>();
        if (testTaxes is not null)
        {
            taxesRepo.MockGetAll(testTaxes);
        }

        var discountsRepo = new MockRepositoryBase<Discount>();
        if (testDiscounts is not null)
        {
            discountsRepo.MockGetAll(testDiscounts);
        }

        _mockApi.Setup(x => x.DocumentsRepo).Returns(docsRepo.Object);
        _mockApi.Setup(x => x.ProductsRepo).Returns(productsRepo.Object);
        _mockApi.Setup(x => x.EntitiesRepo).Returns(entitiesRepo.Object);
        _mockApi.Setup(x => x.LocationsRepo).Returns(locationsRepo.Object);
        _mockApi.Setup(x => x.DocumentTemplatesRepo).Returns(documentTemplatesRepo.Object);
        _mockApi.Setup(x => x.TaxesRepo).Returns(taxesRepo.Object);
        _mockApi.Setup(x => x.DiscountsRepo).Returns(discountsRepo.Object);

        _mockStock.Setup(
                x => x.HandleStockUpdateFromDocumentAction(
                        It.IsAny<Document>(), 
                        It.IsAny<TransactionStockAction>()))
                    .Verifiable();
    }

}
