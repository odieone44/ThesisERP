using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces.Products;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's Products and Services. 
/// </summary>
public class ProductsController : BaseApiController
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductService _productService;

    public ProductsController(ILogger<ProductsController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }


    /// <summary>
    /// Retrieve all products in your account.
    /// </summary>
    /// <response code="200">Returns a list of products.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDTO>))]
    public async Task<IActionResult> GetProducts() //[FromQuery] RequestParams requestParams
    {
        var results = await _productService.GetAllAsync();
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

        var result = await _productService.GetByIdAsync(id);

        if (result == null) { return NotFound(); }
        return Ok(result);
    }

    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <param name="productDTO"></param>
    /// <response code="201">The created product and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="409">If the SKU already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDTO))]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO productDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateProduct)}");
            return BadRequest(ModelState);
        }

        var result = await _productService.CreateAsync(productDTO);

        return CreatedAtRoute("GetProduct", new { id = result.Id }, result);
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

        var product = await _productService.UpdateAsync(id, productDTO);

        if (product == null) { return NotFound(); }

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

        await _productService.DeleteAsync(id);

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

        await _productService.RestoreAsync(id);

        return NoContent();
    }
}
