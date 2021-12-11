using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api;

public class SuppliersController : BaseApiController
{

    private readonly ILogger<SuppliersController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Entity> _entityRepo;

    public SuppliersController(ILogger<SuppliersController> logger, IMapper mapper, IRepositoryBase<Entity> entityRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _entityRepo = entityRepo;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSuppliers()
    {
        var suppliers = await _entityRepo
                            .GetAllAsync
                             (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.supplier,
                                 orderBy: o => o.OrderBy(d => d.DateCreated),
                                 include: i => i.Include(p => p.RelatedProducts));

        var results = _mapper.Map<List<SupplierDTO>>(suppliers);

        return Ok(results);
    }

    [HttpGet("{id:int}", Name = "GetSupplier")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSupplier(int id)
    {
        var supplier = await _getSupplierById(id);

        if (supplier == null) { return NotFound(); }

        var result = _mapper.Map<SupplierDTO>(supplier);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierDTO supplierDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateSupplier)}");
            return BadRequest(ModelState);
        }

        var supplier = _mapper.Map<Entity>(supplierDTO);

        var result = _entityRepo.Add(supplier);

        if (result == null) { return Problem(); }
        
        await _entityRepo.SaveChangesAsync();

        var supplierAdded = _mapper.Map<SupplierDTO>(result);

        return CreatedAtRoute("GetSupplier", new { id = supplierAdded.Id }, supplierAdded);

    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierDTO supplierDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateSupplier)}");
            return BadRequest(ModelState);
        }

        var supplier = await _getSupplierById(id);

        if (supplier == null) { return NotFound(); }

        _mapper.Map(supplierDTO, supplier);
        _entityRepo.Update(supplier);

        await _entityRepo.SaveChangesAsync();

        return NoContent();
    }


    [Authorize()]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        if (id < 1)
        {
            //_logger.LogError($"Invalid Delete Request in {nameof(DeleteSupplier)}");
            return BadRequest("Id has to be provided for Delete action");
        }

        var supplier = await _getSupplierById(id);

        if (supplier == null) { return NotFound(); }

        _entityRepo.Delete(supplier);

        await _entityRepo.SaveChangesAsync();

        return NoContent();

    }

    private async Task<Entity?> _getSupplierById(int id)
    {
        var getSupplier = await _entityRepo
                              .GetAllAsync
                               (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.supplier && x.Id == id,
                                   include: b => b.Include(p => p.RelatedProducts));

        return getSupplier.FirstOrDefault();
    }

}


