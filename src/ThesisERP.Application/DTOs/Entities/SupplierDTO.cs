namespace ThesisERP.Application.DTOs.Entities;

public class SupplierProductsDTO : SupplierDTO
{
    public virtual ICollection<ProductDTO>? RelatedProducts { get; set; } = new List<ProductDTO>();
}

public class SupplierDTO : EntityBaseDTO
{
}

public class CreateSupplierDTO : CreateEntityBaseDTO
{
}

public class UpdateSupplierDTO : UpdateEntityBaseDTO { }

public class SupplierBaseInfoDTO : EntityBasicInfoBaseDTO
{
}