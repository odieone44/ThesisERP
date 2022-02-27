﻿using ThesisERP.Application.DTOs.Transactions.Documents;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.Interfaces.Transactions;

public interface IDocumentService : 
    ITransactionService<GenericDocumentDTO, CreateDocumentDTO, UpdateDocumentDTO, DocumentRowDTO, CreateDocumentRowDTO>
{   
}
