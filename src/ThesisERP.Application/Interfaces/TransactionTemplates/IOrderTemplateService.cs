using ThesisERP.Application.DTOs.Transactions.Orders;

namespace ThesisERP.Application.Interfaces.TransactionTemplates;

public interface IOrderTemplateService
{
    Task<List<OrderTemplateDTO>> GetAllAsync();
    Task<OrderTemplateDTO> GetByIdAsync(int id);
    Task<OrderTemplateDTO> CreateAsync(CreateOrderTemplateDTO templateDTO);
    Task<OrderTemplateDTO> UpdateAsync(int id, UpdateOrderTemplateDTO templateDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
