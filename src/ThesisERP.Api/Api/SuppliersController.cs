using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.Application.Interfaces.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's suppliers.
/// </summary>
public class SuppliersController : BaseApiController
{
    private readonly ILogger<SuppliersController> _logger;
    private readonly ISupplierService _supplierService;

    public SuppliersController(ILogger<SuppliersController> logger, ISupplierService supplierService)
    {
        _logger = logger;
        _supplierService = supplierService;
    }

    /// <summary>
    /// Retrieve all suppliers in your account.
    /// </summary>
    /// <response code="200">Returns a list of suppliers.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SupplierDTO>))]
    public async Task<IActionResult> GetSuppliers() //[FromQuery] RequestParams requestParams
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(suppliers);
    }

    /// <summary>
    /// Retrieve a supplier by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested supplier.</response>
    /// <response code="404">If supplier does not exist.</response>
    [HttpGet("{id:int}", Name = "GetSupplier")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SupplierDTO))]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _supplierService.GetAsync(id);
        if (supplier == null) { return NotFound(); }
        return Ok(supplier);
    }

    /// <summary>
    /// Create a new supplier.
    /// </summary>
    /// <param name="supplierDTO"></param>
    /// <response code="201">The created supplier entity and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SupplierDTO))]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDTO supplierDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateSupplier)}");
            return BadRequest(ModelState);
        }

        var supplier = await _supplierService.CreateAsync(supplierDTO);
        return CreatedAtRoute("GetSupplier", new { id = supplier.Id }, supplier);
    }

    /// <summary>
    /// Update an existing supplier.
    /// </summary>
    /// <param name="supplierDTO">A SupplierDTO with the new values.</param>
    /// <param name="id">The id of the supplier to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the supplier is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO supplierDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateSupplier)}");
            return BadRequest(ModelState);
        }

        var supplier = await _supplierService.UpdateAsync(id, supplierDTO);
        if (supplier == null) { return NotFound(); }
        return NoContent();
    }

    /// <summary>
    /// Soft Delete a supplier.
    /// </summary>
    /// <param name="id">The id of the supplier to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        await _supplierService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Restore a deleted supplier.
    /// </summary>
    /// <param name="id">The id of the supplier to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the supplier is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreSupplier(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        await _supplierService.RestoreAsync(id);
        return NoContent();
    }

}


