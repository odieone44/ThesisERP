using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Transactions;

public static class DocumentExtensions
{
    public async static Task<Document?> GetDocumentByIdIncludeRelations(this IRepositoryBase<Document> docRepo, int documentId)
    {
        var document = (await docRepo
                                .GetAllAsync
                                 (expression: x => x.Id == documentId,
                                  include: i => i.Include(p => p.Entity)
                                                 .Include(x => x.InventoryLocation)
                                                 .Include(o => o.ParentOrder)
                                                 .Include(t => t.Template)
                                                 .Include(q => q.Rows)
                                                     .ThenInclude(d => d.Product)
                                                 .Include(q => q.Rows)
                                                     .ThenInclude(d => d.Tax)
                                                 .Include(q => q.Rows)
                                                     .ThenInclude(d => d.Discount)))
                                .FirstOrDefault();

        return document;
    }
}
