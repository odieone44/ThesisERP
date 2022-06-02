using ThesisERP.Application.DTOs;

namespace ThesisERP.Application.Interfaces;

public interface ILocationService
{
    Task<List<InventoryLocationDTO>> GetAllAsync();
    Task<InventoryLocationDTO> GetByIdAsync(int id);
    Task<InventoryLocationDTO> CreateAsync(CreateInventoryLocationDTO locationDTO);
    Task<InventoryLocationDTO> UpdateAsync(int id, UpdateInventoryLocationDTO locationDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
