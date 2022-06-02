using ThesisERP.Application.DTOs;

namespace ThesisERP.Application.Interfaces.Pricing;

public interface ITaxService
{
    Task<List<TaxDTO>> GetAllAsync();
    Task<TaxDTO> GetByIdAsync(int id);
    Task<TaxDTO> CreateAsync(CreateTaxDTO taxDTO);
    Task<TaxDTO> UpdateAsync(int id, UpdateTaxDTO taxDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);

}
