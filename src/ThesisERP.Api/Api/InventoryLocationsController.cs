using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's Inventory Locations.
/// </summary>
public class InventoryLocationsController : BaseApiController
{
    private readonly ILogger<InventoryLocationsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<InventoryLocation> _locationsRepo;

    public InventoryLocationsController(ILogger<InventoryLocationsController> logger, IMapper mapper, IRepositoryBase<InventoryLocation> locationsRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _locationsRepo = locationsRepo;
    }

    /// <summary>
    /// Retrieve all Inventory Locations in your account.
    /// </summary>
    /// <response code="200">Returns a list of locations.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations() //[FromQuery] RequestParams requestParams
    {
        var locations = await _locationsRepo.GetAllAsync(orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<InventoryLocationDTO>>(locations);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a location by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested location.</response>
    /// <response code="404">If location does not exist.</response>
    [HttpGet("{id:int}", Name = "GetInventoryLocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]    
    public async Task<IActionResult> GetInventoryLocation(int id)
    {
        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        var result = _mapper.Map<InventoryLocationDTO>(location);
        return Ok(result);
    }

    /// <summary>
    /// Create a new Inventory Location.
    /// </summary>
    /// <param name="locationDTO"></param>
    /// <response code="201">The created location and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InventoryLocationDTO))]
    public async Task<IActionResult> CreateInventoryLocation([FromBody] CreateInventoryLocationDTO locationDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateInventoryLocation)}");
            return BadRequest(ModelState);
        }

        var location = _mapper.Map<InventoryLocation>(locationDTO);

        var result = _locationsRepo.Add(location);

        await _locationsRepo.SaveChangesAsync();

        var locationAdded = _mapper.Map<InventoryLocationDTO>(result);

        return CreatedAtRoute("GetInventoryLocation", new { id = locationAdded.Id }, locationAdded);

    }

    /// <summary>
    /// Update an existing location.
    /// </summary>
    /// <param name="locationDTO">A LocationDTO with the new values.</param>
    /// <param name="id">The id of the location to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the location is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateInventoryLocation(int id, [FromBody] UpdateInventoryLocationDTO locationDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateInventoryLocation)}");
            return BadRequest(ModelState);
        }

        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        _mapper.Map(locationDTO, location);

        _locationsRepo.Update(location);

        await _locationsRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Soft Delete a location.
    /// </summary>
    /// <remarks>
    /// As a location is related with other entities, you can mark it as deleted without removing it from the database. <br />
    /// This allows the location deletion to be reverted, and documents that used it to still be accessible for book-keeping purposes.
    /// </remarks>
    /// <param name="id">The id of the location to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the location is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteInventoryLocation(int id)
    {
        if (id < 1)
        {
            //_logger.LogError($"Invalid Delete Request in {nameof(DeleteInventoryLocation)}");
            return BadRequest("Id has to be provided for Delete action");
        }

        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        location.IsDeleted = true;
        _locationsRepo.Update(location);

        //_locationsRepo.Delete(location);
        await _locationsRepo.SaveChangesAsync();

        return NoContent();

    }

    /// <summary>
    /// Restore a deleted location.
    /// </summary>   
    /// <param name="id">The id of the location to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the location is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreInventoryLocation(int id)
    {
        if (id < 1)
        {            
            return BadRequest("Id has to be provided for Restore action");
        }

        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        location.IsDeleted = false;
        _locationsRepo.Update(location);
        
        await _locationsRepo.SaveChangesAsync();

        return NoContent();

    }
}
