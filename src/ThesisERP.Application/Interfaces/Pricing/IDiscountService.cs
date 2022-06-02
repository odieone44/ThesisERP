using ThesisERP.Application.DTOs;

namespace ThesisERP.Application.Interfaces.Pricing;

public interface IDiscountService
{
    Task<List<DiscountDTO>> GetAllAsync();
    Task<DiscountDTO> GetByIdAsync(int id);
    Task<DiscountDTO> CreateAsync(CreateDiscountDTO discountDTO);
    Task<DiscountDTO> UpdateAsync(int id, UpdateDiscountDTO discountDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
