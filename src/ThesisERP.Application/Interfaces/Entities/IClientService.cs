using ThesisERP.Application.DTOs.Entities;

namespace ThesisERP.Application.Interfaces.Entities;

public interface IClientService : IEntityService<ClientDTO, CreateClientDTO, UpdateClientDTO>
{
}
