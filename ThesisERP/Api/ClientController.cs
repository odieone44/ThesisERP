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
    public class ClientController : BaseApiController
    {
        
        private readonly ILogger<ClientController> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryBase<Entity> _entityRepo;

        public ClientController(ILogger<ClientController> logger, IMapper mapper, IRepositoryBase<Entity> entityRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _entityRepo = entityRepo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public async Task<IActionResult> GetClients() //[FromQuery] RequestParams requestParams
        {
            var clients = await _entityRepo
                                .GetAllAsync
                                 (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.client, 
                                     orderBy: o => o.OrderBy(d=>d.DateCreated), 
                                     include: i => i.Include(p=>p.RelatedProducts));

            var results = _mapper.Map<List<ClientDTO>>(clients);

            return Ok(results);
        }

        [HttpGet("{id:int}", Name = "GetClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClient(int id)
        {
            var getClient = await _entityRepo
                                  .GetAllAsync
                                   (expression: x => x.EntityType == Core.Enums.Entities.EntityTypes.client && x.Id == id,
                                       include: b => b.Include(p => p.RelatedProducts));

            var client = getClient.FirstOrDefault();

            if (client == null) { return NotFound(); }

            var result = _mapper.Map<ClientDTO>(client);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDTO clientDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST Request in {nameof(CreateClient)}");
                return BadRequest(ModelState);
            }

            var client = _mapper.Map<Entity>(clientDTO);

            var result = await _entityRepo.AddAsync(client);
            
            var clientAdded = _mapper.Map<ClientDTO>(result);

            return CreatedAtRoute("GetClient", new { id = clientAdded.Id }, clientAdded);

        }

    }
    
}
