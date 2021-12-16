using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Services.Stock;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Get stock information for products or locations.
/// </summary>
public class StockLevelsController : BaseApiController
{
    private readonly ILogger<StockLevelsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<StockLevel> _stockRepo;

    public StockLevelsController(ILogger<StockLevelsController> logger, IMapper mapper, IRepositoryBase<StockLevel> stockRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _stockRepo = stockRepo;
    }

    [HttpGet("GetLocationStock")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetLocationStockDTO>))]
    public async Task<IActionResult> GetLocationStock([FromQuery] int? locationId = null)
    {
        var stock = await _stockRepo.GetLocationStock(locationId);

        return Ok(stock);
    }

    [HttpGet("GetProductStock")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetProductStockDTO>))]
    public async Task<IActionResult> GetProductStock([FromQuery] int? productId = null)
    {
        var stock = await _stockRepo.GetProductStock(productId);

        return Ok(stock);
    }
}
