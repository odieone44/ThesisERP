using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services;

public class ApiService : IApiService
{
    public IRepositoryBase<Document> DocumentsRepo => _documentsRepo;
    public IRepositoryBase<Order> OrdersRepo => _ordersRepo;
    public IRepositoryBase<Product> ProductsRepo => _productsRepo;
    public IRepositoryBase<DocumentTemplate> DocumentTemplatesRepo => _documentTemplatesRepo;
    public IRepositoryBase<OrderTemplate> OrderTemplatesRepo => _orderTemplatesRepo;
    public IRepositoryBase<Entity> EntitiesRepo => _entitiesRepo;
    public IRepositoryBase<InventoryLocation> LocationsRepo => _locationsRepo;
    public IRepositoryBase<StockLevel> StockRepo => _stockRepo;
    public IRepositoryBase<Tax> TaxesRepo => _taxRepo;
    public IRepositoryBase<Discount> DiscountsRepo => _discountRepo;

    private readonly IRepositoryBase<Document> _documentsRepo;
    private readonly IRepositoryBase<Order> _ordersRepo;
    private readonly IRepositoryBase<Product> _productsRepo;
    private readonly IRepositoryBase<DocumentTemplate> _documentTemplatesRepo;
    private readonly IRepositoryBase<OrderTemplate> _orderTemplatesRepo;
    private readonly IRepositoryBase<Entity> _entitiesRepo;
    private readonly IRepositoryBase<InventoryLocation> _locationsRepo;
    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IRepositoryBase<Tax> _taxRepo;
    private readonly IRepositoryBase<Discount> _discountRepo;

    public ApiService(IRepositoryBase<Document> documentsRepo,
                      IRepositoryBase<Order> orderssRepo,
                      IRepositoryBase<Product> productsRepo,
                      IRepositoryBase<DocumentTemplate> documentTmplatesRepo,
                      IRepositoryBase<OrderTemplate> orderTemplatesRepo,
                      IRepositoryBase<Entity> entitiesRepo,
                      IRepositoryBase<InventoryLocation> locationsRepo,
                      IRepositoryBase<StockLevel> stockRepo,
                      IRepositoryBase<Tax> taxRepo,
                      IRepositoryBase<Discount> discountRepo)
    {
        _documentsRepo = documentsRepo;
        _ordersRepo = orderssRepo;
        _productsRepo = productsRepo;
        _documentTemplatesRepo = documentTmplatesRepo;
        _orderTemplatesRepo = orderTemplatesRepo;
        _entitiesRepo = entitiesRepo;
        _locationsRepo = locationsRepo;
        _stockRepo = stockRepo;
        _taxRepo = taxRepo;
        _discountRepo = discountRepo;
    }
}
