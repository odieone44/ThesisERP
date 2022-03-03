using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces;

public interface IApiService
{
    IRepositoryBase<Document> DocumentsRepo { get; }
    IRepositoryBase<Order> Orders { get; }
    IRepositoryBase<Product> ProductsRepo { get; }
    IRepositoryBase<DocumentTemplate> DocumentTemplatesRepo { get; }
    IRepositoryBase<OrderTemplate> OrderTemplatesRepo { get; }
    IRepositoryBase<Entity> EntitiesRepo { get; }
    IRepositoryBase<InventoryLocation> LocationsRepo { get; }
    IRepositoryBase<StockLevel> StockRepo { get; }
    IRepositoryBase<Tax> TaxesRepo { get; }
    IRepositoryBase<Discount> DiscountsRepo { get; }
}
