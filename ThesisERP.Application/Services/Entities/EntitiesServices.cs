using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services.Entities;

public static class EntitiesServices
{
    public async static Task<Entity?> GetClientById(this IRepositoryBase<Entity> entityRepo, int id)
    {
        var getClient = await entityRepo
                              .GetAllAsync
                               (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.client && x.Id == id);

        return getClient.FirstOrDefault();
    }

    public async static Task<Entity?> GetSupplierById(this IRepositoryBase<Entity> entityRepo, int id)
    {
        var getClient = await entityRepo
                              .GetAllAsync
                               (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.client && x.Id == id);

        return getClient.FirstOrDefault();
    }
}
