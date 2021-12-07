using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Xml.Linq;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;
using ThesisERP.Application.Services.Stock;

namespace ThesisERP.Api;

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

    [HttpGet(Name = "GetLocationStock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocationStock([FromQuery] int? locationId = null) //[FromQuery] RequestParams requestParams
    {
       
             
        var stock = await _stockRepo.GetLocationStock(locationId);

        //var results = _mapper.Map<List<StockLevelDTO>>(stock);

        return Ok(stock);
    }

    //[HttpGet("{id:int}", Name = "GetProductStock")]
    //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> GetProductStock(int id, [FromQuery] int? locationId = null)
    //{

    //    if (id < 1)
    //    {
    //        return BadRequest("valid product Id has to be provided");
    //    }

    //    Expression<Func<StockLevel, bool>> queryExp = x => x.ProductId == id && (locationId == null || x.InventoryLocationId == locationId);


    //    var productStock = await _stockRepo
    //                               .GetAllAsync(
    //                                expression: queryExp,
    //                                   orderBy: o => o.OrderBy(d => d.InventoryLocationId).ThenBy(x => x.ProductId),
    //                                   include: include);


    //    if (product == null) { return NotFound(); }

    //    var result = _mapper.Map<ProductDTO>(product);
    //    return Ok(result);
    //}
}
