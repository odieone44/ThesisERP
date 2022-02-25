using ThesisERP.Application.DTOs.Transactions;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;
public interface ITransactionService<T, TCreate, TUpdate>
    where T : TransactionBase
    where TCreate : CreateTransactionBaseDTO
    where TUpdate : UpdateTransactionBaseDTO
{
    Task<T> Create(TCreate createTransactionDTO, string username);
    Task<T> Update(int id, TUpdate updateTransactionDTO);
    Task<T> Fulfill(int id);
    Task<T> Close(int id);
    Task<T> Cancel(int id);
}
