using System.ComponentModel.DataAnnotations;
using ThesisERP.Application.DTOs.Transactions.Documents;

namespace ThesisERP.Application.DTOs.Transactions.Orders;

public class CreateOrderDTO : CreateTransactionBaseDTO
{
    public ICollection<CreateOrderRowDTO> Rows { get; set; } = new List<CreateOrderRowDTO>();
}

public class UpdateOrderDTO : UpdateTransactionBaseDTO
{
    public ICollection<CreateOrderRowDTO>? Rows { get; set; } = new List<CreateOrderRowDTO>();
}
