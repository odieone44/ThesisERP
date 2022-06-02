using ThesisERP.Application.DTOs.Transactions.Documents;

namespace ThesisERP.Application.Interfaces.TransactionTemplates;

public interface IDocumentTemplateService
{
    Task<List<DocumentTemplateDTO>> GetAllAsync();
    Task<DocumentTemplateDTO> GetByIdAsync(int id);
    Task<DocumentTemplateDTO> CreateAsync(CreateDocumentTemplateDTO templateDTO);
    Task<DocumentTemplateDTO> UpdateAsync(int id, UpdateDocumentTemplateDTO templateDTO);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
}
