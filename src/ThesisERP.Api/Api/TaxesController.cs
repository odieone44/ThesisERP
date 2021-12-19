using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage Taxes.
/// </summary>
public class TaxesController : BaseApiController
{
    private readonly ILogger<TaxesController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Tax> _taxRepo;

    public TaxesController(ILogger<TaxesController> logger, IMapper mapper, IRepositoryBase<Tax> taxRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _taxRepo = taxRepo;
    }

    /// <summary>
    /// Retrieve all taxes in your account.
    /// </summary>
    /// <response code="200">Returns a list of taxes.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaxDTO>))]
    public async Task<IActionResult> GetTaxes() //[FromQuery] RequestParams requestParams
    {
        var taxes = await _taxRepo
                            .GetAllAsync
                             (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<TaxDTO>>(taxes);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a tax by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested tax.</response>
    /// <response code="404">If tax does not exist.</response>
    [HttpGet("{id:int}", Name = "GetTax")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaxDTO))]
    public async Task<IActionResult> GetTax(int id)
    {
        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return NotFound(); }

        var result = _mapper.Map<TaxDTO>(tax);
        return Ok(result);
    }

    /// <summary>
    /// Create a new tax.
    /// </summary>
    /// <remarks>
    /// The tax type is percentage pased. <br />
    /// Provided amount should be the percentage expressed as a decimal, eg '0.10' for 10%. 
    /// </remarks>
    /// <param name="taxDTO"></param>
    /// <response code="201">The created tax and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaxDTO))]
    public async Task<IActionResult> CreateTax([FromBody] CreateTaxDTO taxDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateTax)}");
            return BadRequest(ModelState);
        }

        var tax = _mapper.Map<Tax>(taxDTO);

        var result = _taxRepo.Add(tax);
        await _taxRepo.SaveChangesAsync();
        var taxAdded = _mapper.Map<TaxDTO>(result);

        return CreatedAtRoute("GetTax", new { id = taxAdded.Id }, taxAdded);

    }

    /// <summary>
    /// Update an existing tax.
    /// </summary>
    /// <remarks>
    /// Tax amount cannot be updated.
    /// </remarks>
    /// <param name="taxDTO">A Tax DTO with the new values.</param>
    /// <param name="id">The id of the tax to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the tax is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateTax(int id, [FromBody] UpdateTaxDTO taxDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateTax)}");
            return BadRequest(ModelState);
        }

        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return NotFound(); }

        _mapper.Map(taxDTO, tax);

        _taxRepo.Update(tax);

        await _taxRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Soft Delete a tax.
    /// </summary>
    /// <param name="id">The id of the tax to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the tax is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTax(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return NotFound(); }

        tax.IsDeleted = true;

        _taxRepo.Update(tax);
        await _taxRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Restore a deleted tax.
    /// </summary>
    /// <param name="id">The id of the tax to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the tax is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreTax(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        var tax = await _taxRepo.GetByIdAsync(id);

        if (tax == null) { return NotFound(); }

        tax.IsDeleted = false;

        _taxRepo.Update(tax);
        await _taxRepo.SaveChangesAsync();

        return NoContent();

    }

}
