using AutoMapper;
using Microsoft.Extensions.Logging;
using ThesisERP.Application.DTOs.Entities;
using ThesisERP.Application.Interfaces;
using ThesisERP.Application.Interfaces.Entities;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.Services.Entities;

public class ClientService : IClientService
{
    
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Entity> _entityRepo;

    public ClientService(IMapper mapper, IRepositoryBase<Entity> entityRepo)
    {    
        _mapper = mapper;
        _entityRepo = entityRepo;
    }
    public async Task<List<ClientDTO>> GetAllAsync()
    {
        var clients = await _entityRepo
                           .GetAllAsync
                            (expression: x => x.EntityType == EntityType.client,
                                orderBy: o => o.OrderBy(d => d.DateCreated)); //i => i.Include(p => p.RelatedProducts)

        var results = _mapper.Map<List<ClientDTO>>(clients);

        return results;
    }

    public async Task<ClientDTO?> GetAsync(int id)
    {
        var client = await _entityRepo.GetClientById(id);

        if (client == null) { return null; }

        var result = _mapper.Map<ClientDTO>(client);

        return result;
    }

    public async Task<ClientDTO> CreateAsync(CreateClientDTO clientDTO)
    {
        var client = _mapper.Map<Entity>(clientDTO);

        var result = _entityRepo.Add(client);
        await _entityRepo.SaveChangesAsync();

        var clientAdded = _mapper.Map<ClientDTO>(result);
        return clientAdded;
    }

    public async Task<ClientDTO?> UpdateAsync(int id, UpdateClientDTO clientDTO)
    {
        var client = await _entityRepo.GetClientById(id);

        if (client == null) { return null; }

        _mapper.Map(clientDTO, client);

        _entityRepo.Update(client);

        await _entityRepo.SaveChangesAsync();

        return _mapper.Map<ClientDTO>(client);
    }
    public async Task DeleteAsync(int id)
    {
        var client = await _entityRepo.GetClientById(id);

        if (client == null) { return; }

        client.IsDeleted = true;
        _entityRepo.Update(client);
        await _entityRepo.SaveChangesAsync();
    }

    public async Task RestoreAsync(int id)
    {
        var client = await _entityRepo.GetClientById(id);

        if (client == null) { return; }

        client.IsDeleted = false;
        _entityRepo.Update(client);
        await _entityRepo.SaveChangesAsync();
    }
}
