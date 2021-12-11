using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

public class ProductsController : BaseApiController
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Product> _productsRepo;

    public ProductsController(ILogger<ProductsController> logger, IMapper mapper, IRepositoryBase<Product> productsRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _productsRepo = productsRepo;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts() //[FromQuery] RequestParams requestParams
{
        var products = await _productsRepo
                            .GetAllAsync(orderBy: o => o.OrderBy(d => d.Id)); //include: x=>x.Include(p => p.RelatedEntities) //todo: decide if i include related entities.

        var results = _mapper.Map<List<ProductDTO>>(products);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id)
    {

        if (id < 1)
        {
            return BadRequest("valid Id has to be provided");
        }

        var product = await _productsRepo.GetByIdAsync(id);

        if (product == null) { return NotFound(); }

        var result = _mapper.Map<ProductDTO>(product);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDTO))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateProduct)}");
            return BadRequest(ModelState);
        }

        var product = _mapper.Map<Product>(productDTO);

        var result = _productsRepo.Add(product);
        
        await _productsRepo.SaveChangesAsync();

        var productAdded = _mapper.Map<ProductDTO>(result);

        return CreatedAtRoute("GetProduct", new { id = productAdded.Id }, productAdded);

    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateProduct)}");
            return BadRequest(ModelState);
        }

        var product = await _productsRepo.GetByIdAsync(id);
        if (product == null) { return NotFound(); }

        _mapper.Map(productDTO, product);
        _productsRepo.Update(product);

        await _productsRepo.SaveChangesAsync();

        return NoContent();
    }

    [Authorize()]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id < 1)
        {            
            return BadRequest("Id has to be provided for Delete action");
        }

        var product = await _productsRepo.GetByIdAsync(id);

        if (product == null) { return NotFound(); }

        _productsRepo.Delete(product);

        await _productsRepo.SaveChangesAsync();

        return NoContent();

    }
}
