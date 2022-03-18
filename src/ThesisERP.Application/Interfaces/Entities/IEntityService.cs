using ThesisERP.Application.DTOs.Entities;

namespace ThesisERP.Application.Interfaces.Entities;

public interface IEntityService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO>
    where TEntityDTO : EntityBaseDTO
    where TEntityCreateDTO : CreateEntityBaseDTO
    where TEntityUpdateDTO : UpdateEntityBaseDTO
{
    Task<TEntityDTO> CreateAsync(TEntityCreateDTO entityDTO);
    Task<TEntityDTO?> UpdateAsync(int id, TEntityUpdateDTO entityDTO);
    Task<List<TEntityDTO>> GetAllAsync();
    Task<TEntityDTO?> GetAsync(int id);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
