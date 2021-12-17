using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ThesisERP.Application.DTOs;
using ThesisERP.Application.Interfaces;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's clients.
/// </summary>
public class ClientsController : BaseApiController
{

    private readonly ILogger<ClientsController> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Entity> _entityRepo;

    public ClientsController(ILogger<ClientsController> logger, IMapper mapper, IRepositoryBase<Entity> entityRepo)
    {
        _logger = logger;
        _mapper = mapper;
        _entityRepo = entityRepo;
    }

    /// <summary>
    /// Retrieve all clients in your account.
    /// </summary>
    /// <response code="200">Returns a list of clients.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClients() //[FromQuery] RequestParams requestParams
    {
        var clients = await _entityRepo
                            .GetAllAsync
                             (expression: x => x.EntityType == EntityType.client,
                                 orderBy: o => o.OrderBy(d => d.DateCreated)); //i => i.Include(p => p.RelatedProducts)

        var results = _mapper.Map<List<ClientDTO>>(clients);

        return Ok(results);
    }

    /// <summary>
    /// Retrieve a client by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested client.</response>
    /// <response code="404">If client does not exist.</response>
    [HttpGet("{id:int}", Name = "GetClient")]
    [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(ClientDTO))]    
    public async Task<IActionResult> GetClient(int id)
    {
        var client = await _getClientById(id);

        if (client == null) { return NotFound(); }

        var result = _mapper.Map<ClientDTO>(client);
        return Ok(result);
    }

    /// <summary>
    /// Create a new client.
    /// </summary>
    /// <param name="clientDTO"></param>
    /// <response code="201">The created client entity and the route to access it.</response>
    /// <response code="400">If the request body is invalid.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ClientDTO))]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDTO clientDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Request in {nameof(CreateClient)}");
            return BadRequest(ModelState);
        }

        var client = _mapper.Map<Entity>(clientDTO);

        var result = _entityRepo.Add(client);
        await _entityRepo.SaveChangesAsync();
        var clientAdded = _mapper.Map<ClientDTO>(result);

        return CreatedAtRoute("GetClient", new { id = clientAdded.Id }, clientAdded);

    }

    /// <summary>
    /// Update an existing client.
    /// </summary>
    /// <param name="clientDTO">A ClientDTO with the new values.</param>
    /// <param name="id">The id of the client to update</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request body is invalid.</response>
    /// <response code="404">If the client is not found.</response>    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDTO clientDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid PUT Request in {nameof(UpdateClient)}");
            return BadRequest(ModelState);
        }

        var client = await _getClientById(id);

        if (client == null) { return NotFound(); }

        _mapper.Map(clientDTO, client);
        
        _entityRepo.Update(client);

        await _entityRepo.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a client.
    /// </summary>
    /// <param name="id">The id of the client to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the client is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteClient(int id)
    {
        if (id < 1)
        {
            //_logger.LogError($"Invalid Delete Request in {nameof(DeleteClient)}");
            return BadRequest("Id has to be provided for Delete action");
        }

        var client = await _getClientById(id);

        if (client == null) { return NotFound(); }

        _entityRepo.Delete(client);
        
        await _entityRepo.SaveChangesAsync();

        return NoContent();

    }

    private async Task<Entity?> _getClientById(int id)
    {
        var getClient = await _entityRepo
                              .GetAllAsync
                               (expression: x => x.EntityType == EntityType.client && x.Id == id,
                                   include: b => b.Include(p => p.RelatedProducts));

        return getClient.FirstOrDefault();
    }
}


