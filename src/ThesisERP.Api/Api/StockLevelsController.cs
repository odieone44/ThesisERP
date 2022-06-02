using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;

namespace ThesisERP.Api;

/// <summary>
/// Get stock information for products or locations.
/// </summary>
public class StockLevelsController : BaseApiController
{
    private readonly ILogger<StockLevelsController> _logger;
    private readonly IStockService _stockService;

    public StockLevelsController(ILogger<StockLevelsController> logger, IStockService stockService)
    {
        _logger = logger;
        _stockService = stockService;
    }

    /// <summary>
    /// Retrieve stock levels per location.
    /// </summary>
    /// <param name="locationId">Optional. If provided, only stock levels of the required location will be returned</param>
    /// <response code="200">Returns a list of locations with stock information per product.</response>
    [HttpGet("Locations")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetLocationStockDTO>))]
    public async Task<IActionResult> GetLocationStock([FromQuery] int? locationId = null)
    {
        var locationStock = await _stockService.GetLocationStock(locationId);

        return Ok(locationStock);
    }

    /// <summary>
    /// Retrieve stock levels per product.
    /// </summary>
    /// <param name="productId">Optional. If provided, only stock levels of the required product will be returned</param>
    /// <response code="200">Returns a list of products with stock information per location.</response>
    [HttpGet("Products")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetProductStockDTO>))]
    public async Task<IActionResult> GetProductStock([FromQuery] int? productId = null)
    {
        var productStock = await _stockService.GetProductStock(productId);

        return Ok(productStock);
    }
}
