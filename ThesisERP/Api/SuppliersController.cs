using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;

namespace ThesisERP.Api
{    
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
        public async Task<IActionResult> GetSuppliers() //[FromQuery] RequestParams requestParams
        {
            var suppliers = await _entityRepo
                                .GetAllAsync
                                 (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.supplier, 
                                     orderBy: o => o.OrderBy(d=>d.DateCreated), 
                                     include: i => i.Include(p=>p.RelatedProducts));

            var results = _mapper.Map<List<SupplierDTO>>(suppliers);

            return Ok(results);
        }

        [HttpGet("{id:int}", Name = "GetSupplier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var getSupplier = await _entityRepo
                                  .GetAllAsync
                                   (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.supplier && x.Id == id,
                                       include: b => b.Include(p => p.RelatedProducts));

            var supplier = getSupplier.FirstOrDefault();

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

            var result = await _entityRepo.AddAsync(supplier);

            if (result == null) { return Problem(); }
            
            var supplierAdded = _mapper.Map<SupplierDTO>(result);

            return CreatedAtRoute("GetSupplier", new { id = supplierAdded.Id }, supplierAdded);

        }

    }
    
}
