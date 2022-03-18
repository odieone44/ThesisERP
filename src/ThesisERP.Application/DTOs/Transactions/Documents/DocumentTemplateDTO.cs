using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Transactions.Documents;
public class DocumentTemplateDTO : TransactionTemplateBaseDTO
{
    public DocumentType DocumentType { get; set; }
}

public class CreateDocumentTemplateDTO : CreateTransactionTemplateBaseDTO
{
    [Required]
    public DocumentType DocumentType { get; set; }
}

public class UpdateDocumentTemplateDTO : UpdateTransactionTemplateBaseDTO
{

}