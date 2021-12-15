using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api.Api;

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
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTemplates()
    {
        var templates = await _templatesRepo
                            .GetAllAsync(orderBy: o => o.OrderBy(d => d.Id));

        var results = _mapper.Map<List<DocumentTemplateDTO>>(templates);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetTemplate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DocumentTemplateDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    [Authorize]
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

    //[Authorize]
    //[HttpPut("{id:int}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDTO)
    //{
    //    if (!ModelState.IsValid || id < 1)
    //    {
    //        _logger.LogError($"Invalid PUT Request in {nameof(UpdateProduct)}");
    //        return BadRequest(ModelState);
    //    }

    //    var product = await _productsRepo.GetByIdAsync(id);
    //    if (product == null) { return NotFound(); }

    //    _mapper.Map(productDTO, product);
    //    _productsRepo.Update(product);

    //    await _productsRepo.SaveChangesAsync();

    //    return NoContent();
    //}

    //[Authorize()]
    //[HttpDelete("{id:int}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //public async Task<IActionResult> DeleteProduct(int id)
    //{
    //    if (id < 1)
    //    {
    //        return BadRequest("Id has to be provided for Delete action");
    //    }

    //    var product = await _productsRepo.GetByIdAsync(id);

    //    if (product == null) { return NotFound(); }

    //    _productsRepo.Delete(product);

    //    await _productsRepo.SaveChangesAsync();

    //    return NoContent();

    //}
}
