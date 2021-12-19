using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage Discounts.
/// </summary>
public class DiscountsController : BaseApiController
{
    private readonly ILogger<DiscountsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Discount> _discountRepo;

    public DiscountsController(ILogger<DiscountsController> logger, IMapper mapper, IRepositoryBase<Discount> discountRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _discountRepo = discountRepo;
    }

    /// <summary>
    /// Retrieve all discounts in your account.
    /// </summary>
    /// <response code="200">Returns a list of discounts.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DiscountDTO>))]
    public async Task<IActionResult> GetDiscounts() //[FromQuery] RequestParams requestParams
    {
        var discounts = await _discountRepo
                            .GetAllAsync
                             (orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<DiscountDTO>>(discounts);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a discount by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested discount.</response>
    /// <response code="404">If discount does not exist.</response>
    [HttpGet("{id:int}", Name = "GetDiscount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DiscountDTO))]
    public async Task<IActionResult> GetDiscount(int id)
    {
        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return NotFound(); }

        var result = _mapper.Map<DiscountDTO>(discount);
        return Ok(result);
    }

    /// <summary>
    /// Create a new discount.
    /// </summary>
    /// <remarks>
    /// The discount type is percentage pased. <br />
    /// Provided amount should be the percentage expressed as a decimal, eg '0.10' for 10%. 
    /// </remarks>
    /// <param name="discountDTO"></param>
    /// <response code="201">The created discount and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DiscountDTO))]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountDTO discountDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDiscount)}");
            return BadRequest(ModelState);
        }

        var discount = _mapper.Map<Discount>(discountDTO);

        var result = _discountRepo.Add(discount);
        await _discountRepo.SaveChangesAsync();
        var discountAdded = _mapper.Map<DiscountDTO>(result);

        return CreatedAtRoute("GetDiscount", new { id = discountAdded.Id }, discountAdded);

    }

    /// <summary>
    /// Update an existing discount.
    /// </summary>
    /// <remarks>
    /// Discount amount cannnot be updated.
    /// </remarks>
    /// <param name="discountDTO">A Discount DTO with the new values.</param>
    /// <param name="id">The id of the discount to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the discount is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountDTO discountDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateDiscount)}");
            return BadRequest(ModelState);
        }

        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return NotFound(); }

        _mapper.Map(discountDTO, discount);

        _discountRepo.Update(discount);

        await _discountRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Soft Delete a discount.
    /// </summary>
    /// <param name="id">The id of the discount to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the discount is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return NotFound(); }

        discount.IsDeleted = true;

        _discountRepo.Update(discount);
        await _discountRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Restore a deleted discount.
    /// </summary>
    /// <param name="id">The id of the discount to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the discount is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreDiscount(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        var discount = await _discountRepo.GetByIdAsync(id);

        if (discount == null) { return NotFound(); }

        discount.IsDeleted = false;

        _discountRepo.Update(discount);
        await _discountRepo.SaveChangesAsync();

        return NoContent();

    }
}
