using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Application.Interfaces.Transactions;

namespace ThesisERP.Api;

/// <summary>
/// Issue and manage Invoices, Bills and other business documents.
/// </summary>
public class DocumentsController : BaseApiController
{
    private readonly ILogger<DocumentsController> _logger;
    private readonly IDocumentService _documentService;

    public DocumentsController(ILogger<DocumentsController> logger, IDocumentService docService)
    {
        _logger = logger;
        _documentService = docService;
    }

    /// <summary>
    /// Create a new Document in pending state.
    /// </summary>
    /// <remarks>
    /// Creates a new document in the provided Inventory Location, for the provided Supplier / Client, containing the specified product rows.
    /// </remarks>
    /// <param name="documentDTO"></param>
    /// <response code="200">Returns the created document.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDTO documentDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDocument)}");
            return BadRequest(ModelState);
        }

        var username = HttpContext.User.Identity?.Name ?? string.Empty;

        var response = await _documentService.CreateAsync(documentDTO, username);

        return Ok(response);
    }


    /// <summary>
    /// Update a document.
    /// </summary>
    /// <remarks>
    /// Only 'Pending' and 'Fulfilled' documents can be updated. <br />
    /// The Document Template cannot be updated. <br />
    /// Only documents in 'Pending' state can have their supplier/client, location and product rows updated. <br />    
    /// </remarks>
    /// <param name="id">The id of the document to update.</param>
    /// <param name="documentDTO">The new values of the document.</param>
    /// <response code="200">Returns the updated document.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateDocumentDTO documentDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(UpdateDocument)}");
            return BadRequest(ModelState);
        }

        var response = await _documentService.UpdateAsync(id, documentDTO);

        return Ok(response);
    }

    /// <summary>
    /// Fulfill a document.
    /// </summary>
    /// <param name="id">The id of the document to fulfill</param>
    /// <remarks>
    /// Document needs to be in pending status to be fulfilled. <br />
    /// Upon fulfilling, the document Entity, Location and Rows cannot be updated, and all related physical stock level updates take place.
    /// </remarks>
    /// <response code="200">Returns the fulfilled document.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Fulfill")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> FulfillDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var response = await _documentService.FulfillAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Close a document.
    /// </summary>
    /// <remarks>
    /// Closing a document means it is considered completed, and no further updates can take place.
    /// </remarks>
    /// <param name="id">The id of the document to close</param>
    /// <response code="200">Returns the closed document.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Close")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> CloseDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var response = await _documentService.CloseAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Cancel a document.
    /// </summary>
    /// <remarks>
    /// Canceling a document will undo any changes resulting from its creation/fulfillment.
    /// </remarks>
    /// <param name="id">The id of the document to cancel</param>
    /// <response code="200">Returns the cancelled document.</response>
    /// <response code="400">If request is not valid.</response>
    [HttpPost("{id:int}/Cancel")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> CancelDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var response = await _documentService.CancelAsync(id);
        return Ok(response);
    }

    /// <summary>
    /// Retrieve all documents.
    /// </summary>
    /// <response code="200">A list of all documents in your account.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GenericDocumentDTO>))]
    public async Task<IActionResult> GetDocuments()
    {

        var documents = await _documentService.GetDocumentsAsync();

        return Ok(documents);
    }

    /// <summary>
    /// Retrieve a document by id
    /// </summary>
    /// <param name="id">The id of the document to retrieve</param>
    /// <response code="200">Returns the requested document.</response>
    /// <response code="404">If document does not exist.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericDocumentDTO))]
    public async Task<IActionResult> GetDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var document = await _documentService.GetDocumentAsync(id);

        if (document == null) { return NotFound(); }

        return Ok(document);
    }

}
