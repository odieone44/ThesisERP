using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Transactions;

public static class OrderExtensions
{

    public async static Task<Order?> GetOrderByIdIncludeRelations(this IRepositoryBase<Order> orderRepo, int orderId)
    {
        var order = (await orderRepo
                               .GetAllAsync
                                (expression: x => x.Id == orderId,
                                 include: i => i.Include(p => p.Entity)
                                                .Include(x => x.InventoryLocation)                                                
                                                .Include(t => t.Template)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Product)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Tax)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Discount)
                                                .Include(x=> x.RelatedDocuments)))                                                    
                               .FirstOrDefault();

        return order;
    }

}
