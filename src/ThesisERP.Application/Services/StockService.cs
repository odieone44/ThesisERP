using AutoMapper;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Services.Stock;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Services;

public class StockService : IStockService
{
    private readonly IRepositoryBase<StockLevel> _stockRepo;
    private readonly IMapper _mapper;

    public StockService(IRepositoryBase<StockLevel> stockRepo, IMapper mapper)
    {
        _stockRepo = stockRepo;
        _mapper = mapper;
    }

    public async Task<List<GetLocationStockDTO>> GetLocationStock(int? locationId)
    {
        return await _stockRepo.GetLocationStock(locationId);
    }

    public async Task<List<GetProductStockDTO>> GetProductStock(int? productId)
    {
        return await _stockRepo.GetProductStock(productId);
    }

    public async Task<UpdateProductStockDTO> UpdateProductStock(UpdateProductStockDTO updateStockDto)
    {
        throw new NotImplementedException();
    }
}
