using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

public class DocumentsController : BaseApiController
{
    private readonly ILogger<DocumentsController> _logger;
    private readonly IMapper _mapper;    
    private readonly IDocumentService _documentService;

    public DocumentsController(ILogger<DocumentsController> logger, IMapper mapper, IDocumentService docService)
    {
        _logger = logger;
        _mapper = mapper;
        _documentService = docService;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentDTO documentDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateDocument)}");
            return BadRequest(ModelState);
        }

        var username = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value ?? string.Empty;
        //HttpContext.User.Identity.Name
        var document = await _documentService.Create(documentDTO, username);        

        return Ok(document);

    }
}
