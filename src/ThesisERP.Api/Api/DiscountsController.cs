namespace ThesisERP.Api.Api;

using Microsoft.AspNetCore.Mvc;

using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces.Pricing;


/// <summary>
/// Manage Discounts.
/// </summary>
public class DiscountsController : BaseApiController
{
    private readonly ILogger<DiscountsController> _logger;
    private readonly IDiscountService _discountService;

    public DiscountsController(ILogger<DiscountsController> logger, IDiscountService discountService)
    {
        _logger = logger;
        _discountService = discountService;
    }

    /// <summary>
    /// Retrieve all discounts in your account.
    /// </summary>
    /// <response code="200">Returns a list of discounts.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DiscountDTO>))]
    public async Task<IActionResult> GetDiscounts() //[FromQuery] RequestParams requestParams
    {
        var discounts = await _discountService.GetAllAsync();
        return Ok(discounts);
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
        var discount = await _discountService.GetByIdAsync(id);

        if (discount is null) { return NotFound(); }

        return Ok(discount);
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

        var discountAdded = await _discountService.CreateAsync(discountDTO);

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

        var result = await _discountService.UpdateAsync(id, discountDTO);

        if (result is null) { return NotFound(); }

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

        await _discountService.DeleteAsync(id);

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

        await _discountService.RestoreAsync(id);

        return NoContent();

    }
}
