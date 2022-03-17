using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Orders;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage the Templates used to issue orders.
/// </summary>
public class OrderTemplatesController : BaseApiController
{
    private readonly ILogger<OrderTemplatesController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<OrderTemplate> _templatesRepo;

    public OrderTemplatesController(ILogger<OrderTemplatesController> logger, IMapper mapper, IRepositoryBase<OrderTemplate> templatesRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _templatesRepo = templatesRepo;
    }

    /// <summary>
    /// Retrieve all order templates in your account.
    /// </summary>
    /// <response code="200">Returns a list of order templates.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderTemplateDTO>))]
    public async Task<IActionResult> GetOrderTemplates()
    {
        var templates = await _templatesRepo
                            .GetAllAsync(orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<OrderTemplateDTO>>(templates);

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

        var template = await _templatesRepo.GetByIdAsync(id);

        if (template == null) { return NotFound(); }

        var result = _mapper.Map<OrderTemplateDTO>(template);
        return Ok(result);
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

        var template = _mapper.Map<OrderTemplate>(templateDTO);

        var result = _templatesRepo.Add(template);

        await _templatesRepo.SaveChangesAsync();

        var templateAdded = _mapper.Map<OrderTemplateDTO>(result);

        return CreatedAtRoute("GetOrderTemplate", new { id = templateAdded.Id }, templateAdded);
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

        var template = await _templatesRepo.GetByIdAsync(id);
        if (template == null) { return NotFound(); }

        _mapper.Map(templateDTO, template);
        _templatesRepo.Update(template);

        await _templatesRepo.SaveChangesAsync();
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

        var template = await _templatesRepo.GetByIdAsync(id);

        if (template == null) { return NotFound(); }

        template.IsDeleted = true;

        _templatesRepo.Update(template);

        await _templatesRepo.SaveChangesAsync();

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

        var template = await _templatesRepo.GetByIdAsync(id);

        if (template == null) { return NotFound(); }

        template.IsDeleted = false;

        _templatesRepo.Update(template);

        await _templatesRepo.SaveChangesAsync();

        return NoContent();

    }
}
