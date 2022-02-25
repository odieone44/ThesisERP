using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage the Templates used to issue documents.
/// </summary>
public class TemplatesController : BaseApiController
{
    private readonly ILogger<TemplatesController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<DocumentTemplate> _templatesRepo;

    public TemplatesController(ILogger<TemplatesController> logger, IMapper mapper, IRepositoryBase<DocumentTemplate> templatesRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _templatesRepo = templatesRepo;
    }

    /// <summary>
    /// Retrieve all document templates in your account.
    /// </summary>
    /// <response code="200">Returns a list of document templates.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentTemplateDTO>))]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _templatesRepo
                            .GetAllAsync(orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<DocumentTemplateDTO>>(templates);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a document template by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested template.</response>
    /// <response code="404">If template does not exist.</response>
    [HttpGet("{id:int}", Name = "GetTemplate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDTO))]
    public async Task<IActionResult> GetTemplate(int id)
    {

        if (id < 1)
        {
            return BadRequest("valid Id has to be provided");
        }

        var template = await _templatesRepo.GetByIdAsync(id);

        if (template == null) { return NotFound(); }

        var result = _mapper.Map<DocumentTemplateDTO>(template);
        return Ok(result);
    }

    /// <summary>
    /// Create a new document template.
    /// </summary>
    /// <param name="templateDTO"></param>
    /// <response code="201">The created template and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DocumentTemplateDTO))]
    public async Task<IActionResult> CreateTemplate([FromBody] CreateDocumentTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateTemplate)}");
            return BadRequest(ModelState);
        }

        var template = _mapper.Map<DocumentTemplate>(templateDTO);

        var result = _templatesRepo.Add(template);

        await _templatesRepo.SaveChangesAsync();

        var templateAdded = _mapper.Map<DocumentTemplateDTO>(result);

        return CreatedAtRoute("GetTemplate", new { id = templateAdded.Id }, templateAdded);
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
    public async Task<IActionResult> UpdateTemplate(int id, [FromBody] UpdateDocumentTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateTemplate)}");
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
    public async Task<IActionResult> DeleteTemplate(int id)
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
    public async Task<IActionResult> RestoreTemplate(int id)
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
