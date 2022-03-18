using AutoMapper;
using Microsoft.Extensions.Logging;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Entities;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Services.Entities;

public class SupplierService : ISupplierService
{
    private readonly ILogger<SupplierService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Entity> _entityRepo;

    public SupplierService(ILogger<SupplierService> logger, IMapper mapper, IRepositoryBase<Entity> entityRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _entityRepo = entityRepo;
    }
    public async Task<List<SupplierDTO>> GetAllAsync()
    {
        var suppliers = await _entityRepo
                           .GetAllAsync
                            (expression: x => x.EntityType == EntityType.supplier,
                                orderBy: o => o.OrderBy(d => d.DateCreated)); //i => i.Include(p => p.RelatedProducts)

        var results = _mapper.Map<List<SupplierDTO>>(suppliers);

        return results;
    }

    public async Task<SupplierDTO?> GetAsync(int id)
    {
        var supplier = await _entityRepo.GetSupplierById(id);

        if (supplier == null) { return null; }

        var result = _mapper.Map<SupplierDTO>(supplier);

        return result;
    }

    public async Task<SupplierDTO> CreateAsync(CreateSupplierDTO supplierDTO)
    {
        var supplier = _mapper.Map<Entity>(supplierDTO);

        var result = _entityRepo.Add(supplier);
        await _entityRepo.SaveChangesAsync();

        var supplierAdded = _mapper.Map<SupplierDTO>(result);
        return supplierAdded;
    }

    public async Task<SupplierDTO?> UpdateAsync(int id, UpdateSupplierDTO supplierDTO)
    {
        var supplier = await _entityRepo.GetSupplierById(id);

        if (supplier == null) { return null; }

        _mapper.Map(supplierDTO, supplier);

        _entityRepo.Update(supplier);

        await _entityRepo.SaveChangesAsync();

        return _mapper.Map<SupplierDTO>(supplier);
    }
    public async Task DeleteAsync(int id)
    {
        var supplier = await _entityRepo.GetSupplierById(id);

        if (supplier == null) { return; }

        supplier.IsDeleted = true;
        _entityRepo.Update(supplier);
        await _entityRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var supplier = await _entityRepo.GetSupplierById(id);

        if (supplier == null) { return; }

        supplier.IsDeleted = false;
        _entityRepo.Update(supplier);
        await _entityRepo.SaveChangesAsync();
    }
}
