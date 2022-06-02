using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces.TransactionTemplates;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage the Templates used to issue orders.
/// </summary>
public class OrderTemplatesController : BaseApiController
{
    private readonly ILogger<OrderTemplatesController> _logger;
    private readonly IOrderTemplateService _templateService;

    public OrderTemplatesController(ILogger<OrderTemplatesController> logger, IOrderTemplateService templateService)
    {
        _logger = logger;
        _templateService = templateService;
    }

    /// <summary>
    /// Retrieve all order templates in your account.
    /// </summary>
    /// <response code="200">Returns a list of order templates.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderTemplateDTO>))]
    public async Task<IActionResult> GetOrderTemplates()
    {

        var results = await _templateService.GetAllAsync();
        return Ok(results);
    }

    /// <summary>
    /// Retrieve an order template by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested template.</response>
    /// <response code="404">If template does not exist.</response>
    [HttpGet("{id:int}", Name = "GetOrderTemplate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderTemplateDTO))]
    public async Task<IActionResult> GetOrderTemplate(int id)
    {

        if (id < 1)
        {
            return BadRequest("valid Id has to be provided");
        }

        var template = await _templateService.GetByIdAsync(id);

        if (template == null) { return NotFound(); }

        return Ok(template);
    }

    /// <summary>
    /// Create a new order template.
    /// </summary>
    /// <param name="templateDTO"></param>
    /// <response code="201">The created template and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderTemplateDTO))]
    public async Task<IActionResult> CreateOrderTemplate([FromBody] CreateOrderTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateOrderTemplate)}");
            return BadRequest(ModelState);
        }

        var result = await _templateService.CreateAsync(templateDTO);

        return CreatedAtRoute("GetOrderTemplate", new { id = result.Id }, result);
    }

    /// <summary>
    /// Update an existing template.
    /// </summary>
    /// <remarks>
    /// Template type cannot be updated.
    /// </remarks>
    /// <param name="templateDTO">A Template DTO with the new values.</param>
    /// <param name="id">The id of the template to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the template is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOrderTemplate(int id, [FromBody] UpdateOrderTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateOrderTemplate)}");
            return BadRequest(ModelState);
        }

        var template = await _templateService.UpdateAsync(id, templateDTO);
        if (template == null) { return NotFound(); }

        return NoContent();
    }

    /// <summary>
    /// Soft Delete a template.
    /// </summary>
    /// <param name="id">The id of the template to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the template is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrderTemplate(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        await _templateService.DeleteAsync(id);

        return NoContent();

    }

    /// <summary>
    /// Restore a deleted template.
    /// </summary>
    /// <param name="id">The id of the template to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the template is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreOrderTemplate(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        await _templateService.RestoreAsync(id);

        return NoContent();

    }
}
