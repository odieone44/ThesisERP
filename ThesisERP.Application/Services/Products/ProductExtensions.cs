using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Products;

public static class ProductExtensions
{
    public async static Task<Product?> GetProductBySku(this IRepositoryBase<Product> productRepo, string sku)
    {
        var result = await productRepo.GetAllAsync(p=>p.SKU == sku);

        return result.FirstOrDefault();
    }
}
