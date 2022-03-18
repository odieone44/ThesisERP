namespace ThesisERP.Application.DTOs.Entities;

public class ClientProductsDTO : ClientDTO
{
    public virtual ICollection<ProductDTO>? RelatedProducts { get; set; } = new List<ProductDTO>();
}

public class ClientDTO : EntityBaseDTO
{
}

public class CreateClientDTO : CreateEntityBaseDTO
{
}

public class UpdateClientDTO : UpdateEntityBaseDTO
{
}

public class ClientBaseInfoDTO : EntityBasicInfoBaseDTO
{
}