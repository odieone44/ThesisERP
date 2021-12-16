using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Application.Services.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Issue and manage Invoices, Bills and other business documents.
/// </summary>
public class DocumentsController : BaseApiController
{
    private readonly ILogger<DocumentsController> _logger;
    private readonly IMapper _mapper;
    private readonly IDocumentService _documentService;
    private readonly IRepositoryBase<Document> _docsRepo;

    public DocumentsController(ILogger<DocumentsController> logger, IMapper mapper, IDocumentService docService, IRepositoryBase<Document> docsRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _documentService = docService;
        _docsRepo = docsRepo;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDTO documentDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDocument)}");
            return BadRequest(ModelState);
        }

        var username = HttpContext.User.Identity?.Name ?? string.Empty;

        var document = await _documentService.Create(documentDTO, username);

        var response = _mapper.Map<DocumentDTO>(document);
        return Ok(response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDocument(int id, [FromBody] UpdateDocumentDTO documentDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDocument)}");
            return BadRequest(ModelState);
        }

        var username = HttpContext.User.Identity?.Name ?? string.Empty;

        var document = await _documentService.Update(id, documentDTO);

        var response = _mapper.Map<DocumentDTO>(document);
        return Ok(response);
    }

    [HttpPost("Fulfill/{id:int}")]
    public async Task<IActionResult> FulfillDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var document = await _documentService.Fulfill(id);

        var response = _mapper.Map<DocumentDTO>(document);
        return Ok(response);
    }

    [HttpPost("Close/{id:int}")]
    public async Task<IActionResult> CloseDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var document = await _documentService.Close(id);

        var response = _mapper.Map<DocumentDTO>(document);
        return Ok(response);
    }

    [HttpPost("Cancel/{id:int}")]
    public async Task<IActionResult> CancelDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var document = await _documentService.Cancel(id);

        var response = _mapper.Map<DocumentDTO>(document);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DocumentDTO>))]
    public async Task<IActionResult> GetDocuments()
    {
        var documents = await _docsRepo
                                .GetAllAsync
                                 (orderBy: o => o.OrderByDescending(d => d.DateUpdated),
                                 include: i => i.Include(p => p.Entity)
                                                .Include(x => x.InventoryLocation)
                                                .Include(t => t.DocumentTemplate)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Product)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Tax)
                                                .Include(q => q.Rows)
                                                    .ThenInclude(d => d.Discount));

        var results = _mapper.Map<List<DocumentDTO>>(documents);

        return Ok(results);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentDTO))]
    public async Task<IActionResult> GetDocument(int id)
    {
        if (id < 1)
        {
            return BadRequest("Document Id has to be provided.");
        }

        var document = await _docsRepo.GetDocumentByIdIncludeRelations(id);

        if (document == null) { return NotFound(); }

        var result = _mapper.Map<DocumentDTO>(document);

        return Ok(result);
    }

}
