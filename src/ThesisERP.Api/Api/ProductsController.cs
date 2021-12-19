using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's Products and Services. 
/// </summary>
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


    /// <summary>
    /// Retrieve all products in your account.
    /// </summary>
    /// <response code="200">Returns a list of products.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(List<ProductDTO>))]
    public async Task<IActionResult> GetProducts() //[FromQuery] RequestParams requestParams
{
        var products = await _productsRepo
                            .GetAllAsync(orderBy: o => o.OrderBy(d => d.Id)); //include: x=>x.Include(p => p.RelatedEntities) //todo: decide if i include related entities.

        var results = _mapper.Map<List<ProductDTO>>(products);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a product by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested product.</response>
    /// <response code="404">If product does not exist.</response>
    [HttpGet("{id:int}", Name = "GetProduct")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]    
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

    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="productDTO"></param>
    /// <response code="201">The created product and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
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

    /// <summary>
    /// Update an existing product.
    /// </summary>
    /// <param name="productDTO">A ProductDTO with the new values.</param>
    /// <param name="id">The id of the product to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the product is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]    
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


    /// <summary>
    /// Soft Delete a product.
    /// </summary>
    /// <param name="id">The id of the product to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]    
    public async Task<IActionResult> DeleteProduct(int id)
    {
        if (id < 1)
        {            
            return BadRequest("Id has to be provided for Delete action");
        }

        var product = await _productsRepo.GetByIdAsync(id);

        if (product == null) { return NotFound(); }

        product.IsDeleted = true;
        //_productsRepo.Delete(product);
        _productsRepo.Update(product);

        await _productsRepo.SaveChangesAsync();

        return NoContent();

    }

    /// <summary>
    /// Restore a deleted product.
    /// </summary>
    /// <param name="id">The id of the product to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the product is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreProduct(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        var product = await _productsRepo.GetByIdAsync(id);

        if (product == null) { return NotFound(); }

        product.IsDeleted = false;        
        _productsRepo.Update(product);

        await _productsRepo.SaveChangesAsync();

        return NoContent();
    }
}
