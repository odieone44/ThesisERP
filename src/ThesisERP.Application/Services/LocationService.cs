using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services;

public class LocationService : ILocationService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<InventoryLocation> _locationRepo;
    public LocationService(IMapper mapper, IRepositoryBase<InventoryLocation> locationRepo)
    {
        _mapper = mapper;
        _locationRepo = locationRepo;
    }

    public async Task<InventoryLocationDTO> CreateAsync(CreateInventoryLocationDTO locationDTO)
    {
        var locationToAdd = _mapper.Map<InventoryLocation>(locationDTO);
        var result = _locationRepo.Add(locationToAdd);

        await _locationRepo.SaveChangesAsync();

        return _mapper.Map<InventoryLocationDTO>(result);
    }

    public async Task<InventoryLocationDTO> UpdateAsync(int id, UpdateInventoryLocationDTO locationDTO)
    {
        var location = await _locationRepo.GetByIdAsync(id);

        if (location == null) { return null; }

        _mapper.Map(locationDTO, location);

        _locationRepo.Update(location);

        await _locationRepo.SaveChangesAsync();

        return _mapper.Map<InventoryLocationDTO>(location);
    }

    public async Task<List<InventoryLocationDTO>> GetAllAsync()
    {
        var locations = await _locationRepo
                           .GetAllAsync
                            (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<InventoryLocationDTO>>(locations);

        return results;
    }

    public async Task<InventoryLocationDTO> GetByIdAsync(int id)
    {
        var location = await _locationRepo.GetByIdAsync(id);

        if (location == null) { return null; }

        return _mapper.Map<InventoryLocationDTO>(location);
    }

    public async Task DeleteAsync(int id)
    {
        var location = await _locationRepo.GetByIdAsync(id);

        if (location == null) { return; }

        location.IsDeleted = true;

        _locationRepo.Update(location);
        await _locationRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var location = await _locationRepo.GetByIdAsync(id);

        if (location == null) { return; }

        location.IsDeleted = false;

        _locationRepo.Update(location);
        await _locationRepo.SaveChangesAsync();
    }
}
