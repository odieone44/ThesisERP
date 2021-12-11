using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

[Route("api/[controller]")]
[ApiController]
public class InventoryLocationsController : ControllerBase
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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocations() //[FromQuery] RequestParams requestParams
    {
        var locations = await _locationsRepo.GetAllAsync(orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<InventoryLocationDTO>>(locations);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetInventoryLocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventoryLocation(int id)
    {
        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        var result = _mapper.Map<InventoryLocationDTO>(location);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
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

    [Authorize]
    [HttpPut("{id:int}")]
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

    [Authorize()]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInventoryLocation(int id)
    {
        if (id < 1)
        {
            //_logger.LogError($"Invalid Delete Request in {nameof(DeleteInventoryLocation)}");
            return BadRequest("Id has to be provided for Delete action");
        }

        var location = await _locationsRepo.GetByIdAsync(id);

        if (location == null) { return NotFound(); }

        _locationsRepo.Delete(location);

        await _locationsRepo.SaveChangesAsync();

        return NoContent();

    }
}
