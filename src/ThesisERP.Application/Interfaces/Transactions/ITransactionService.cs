using ThesisERP.Application.DTOs.Transactions;

namespace ThesisERP.Application.Interfaces.Transactions;
public interface ITransactionService<TTransactionDTO, TCreateDTO, TUpdateDTO, TRowDTO, TCreateRowDTO>
    where TTransactionDTO : TransactionBaseDTO<TRowDTO>
    where TCreateDTO : CreateTransactionBaseDTO<TCreateRowDTO>
    where TUpdateDTO : UpdateTransactionBaseDTO<TCreateRowDTO>
    where TRowDTO : TransactionRowBaseDTO
    where TCreateRowDTO : CreateTransactionRowBaseDTO
{
    Task<TTransactionDTO> Create(TCreateDTO createTransactionDTO, string username);
    Task<TTransactionDTO> Update(int id, TUpdateDTO updateTransactionDTO);
    Task<TTransactionDTO> Fulfill(int id);
    Task<TTransactionDTO> Close(int id);
    Task<TTransactionDTO> Cancel(int id);
}
