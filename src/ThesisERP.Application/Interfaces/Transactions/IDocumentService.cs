using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IDocumentService 
{
    Task<GenericDocumentDTO> CreateAsync(CreateDocumentDTO createTransactionDTO, string username, Order? parentOrder = null);
    Task<GenericDocumentDTO> UpdateAsync(int id, UpdateDocumentDTO updateTransactionDTO);
    Task<GenericDocumentDTO> FulfillAsync(int id);
    Task<GenericDocumentDTO> CloseAsync(int id);
    Task<GenericDocumentDTO> CancelAsync(int id);
    Task<List<GenericDocumentDTO>> GetDocumentsAsync();
    Task<GenericDocumentDTO?> GetDocumentAsync(int id);
}
