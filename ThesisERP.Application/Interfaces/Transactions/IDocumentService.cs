using ThesisERP.Application.DTOs.Documents;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IDocumentService
{
    Task<Document> Create(CreateDocumentDTO documentDTO, string username);
    Task<Document> Update(int id, UpdateDocumentDTO documentDTO);
    Task<Document> Fulfill(int id);
    Task<Document> Close(int id);
    Task<Document> Cancel(int id);
}
