using ThesisERP.Application.DTOs;

namespace ThesisERP.Application.Interfaces;

public interface IStockService
{
    Task<UpdateProductStockDTO> UpdateProductStock(UpdateProductStockDTO updateStockDto);

    Task<List<GetLocationStockDTO>> GetLocationStock(int? locationId);

    Task<List<GetProductStockDTO>> GetProductStock(int? productId);

}
