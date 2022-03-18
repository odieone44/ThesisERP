using Microsoft.AspNetCore.Mvc;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.Application.Interfaces.Entities;

namespace ThesisERP.Api;

/// <summary>
/// Manage your organization's clients.
/// </summary>
public class ClientsController : BaseApiController
{
    private readonly ILogger<ClientsController> _logger;
    private readonly IClientService _clientService;

    public ClientsController(ILogger<ClientsController> logger, IClientService clientService)
    {
        _logger = logger;
        _clientService = clientService;
    }

    /// <summary>
    /// Retrieve all clients in your account.
    /// </summary>
    /// <response code="200">Returns a list of clients.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ClientDTO>))]
    public async Task<IActionResult> GetClients() //[FromQuery] RequestParams requestParams
    {
        var clients = await _clientService.GetAllAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Retrieve a client by id.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <response code="200">Returns the requested client.</response>
    /// <response code="404">If client does not exist.</response>
    [HttpGet("{id:int}", Name = "GetClient")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ClientDTO))]
    public async Task<IActionResult> GetClient(int id)
    {
        var client = await _clientService.GetAsync(id);
        if (client == null) { return NotFound(); }
        return Ok(client);
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

        var client = await _clientService.CreateAsync(clientDTO);
        return CreatedAtRoute("GetClient", new { id = client.Id }, client);
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

        var client = await _clientService.UpdateAsync(id, clientDTO);
        if (client == null) { return NotFound(); }
        return NoContent();
    }

    /// <summary>
    /// Soft Delete a client.
    /// </summary>
    /// <param name="id">The id of the client to delete</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteClient(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Delete action");
        }

        await _clientService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Restore a deleted client.
    /// </summary>
    /// <param name="id">The id of the client to restore</param>
    /// <response code="204">On success</response>
    /// <response code="400">If the request is invalid.</response>
    /// <response code="404">If the client is not found.</response>
    [HttpPut("{id:int}/Restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestoreClient(int id)
    {
        if (id < 1)
        {
            return BadRequest("Id has to be provided for Restore action");
        }

        await _clientService.RestoreAsync(id);
        return NoContent();
    }

}


