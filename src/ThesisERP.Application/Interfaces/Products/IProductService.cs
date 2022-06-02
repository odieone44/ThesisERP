using ThesisERP.Application.DTOs;

namespace ThesisERP.Application.Interfaces.Products;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllAsync();
    Task<ProductDTO> GetByIdAsync(int id);
    Task<ProductDTO> CreateAsync(CreateProductDTO productDTO);
    Task<ProductDTO> UpdateAsync(int id, UpdateProductDTO productDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
