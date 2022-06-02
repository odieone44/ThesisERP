using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces.TransactionTemplates;

namespace ThesisERP.Api.Api;

/// <summary>
/// Manage the Templates used to issue documents.
/// </summary>
public class DocumentTemplatesController : BaseApiController
{
    private readonly ILogger<DocumentTemplatesController> _logger;
    private readonly IDocumentTemplateService _templateService;

    public DocumentTemplatesController(ILogger<DocumentTemplatesController> logger, IDocumentTemplateService templateService)
    {
        _logger = logger;
        _templateService = templateService;
    }

    /// <summary>
    /// Retrieve all document templates in your account.
    /// </summary>
    /// <response code="200">Returns a list of document templates.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentTemplateDTO>))]
    public async Task<IActionResult> GetDocumentTemplates()
    {
        var results = await _templateService.GetAllAsync();
        return Ok(results);
    }

    /// <summary>
    /// Retrieve a document template by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested template.</response>
    /// <response code="404">If template does not exist.</response>
    [HttpGet("{id:int}", Name = "GetDocumentTemplate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDTO))]
    public async Task<IActionResult> GetDocumentTemplate(int id)
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
    /// Create a new document template.
    /// </summary>
    /// <param name="templateDTO"></param>
    /// <response code="201">The created template and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DocumentTemplateDTO))]
    public async Task<IActionResult> CreateDocumentTemplate([FromBody] CreateDocumentTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDocumentTemplate)}");
            return BadRequest(ModelState);
        }

        var result = await _templateService.CreateAsync(templateDTO);

        return CreatedAtRoute("GetDocumentTemplate", new { id = result.Id }, result);
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
    public async Task<IActionResult> UpdateDocumentTemplate(int id, [FromBody] UpdateDocumentTemplateDTO templateDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateDocumentTemplate)}");
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
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteDocumentTemplate(int id)
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
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreDocumentTemplate(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        await _templateService.RestoreAsync(id);

        return NoContent();

    }
}
