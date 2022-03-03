using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IDocumentService 
{
    Task<GenericDocumentDTO> Create(CreateDocumentDTO createTransactionDTO, string username);
    Task<GenericDocumentDTO> Update(int id, UpdateDocumentDTO updateTransactionDTO);
    Task<GenericDocumentDTO> Fulfill(int id);
    Task<GenericDocumentDTO> Close(int id);
    Task<GenericDocumentDTO> Cancel(int id);
}
