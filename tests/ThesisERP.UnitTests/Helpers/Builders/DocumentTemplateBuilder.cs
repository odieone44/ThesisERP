using System;
using System.Collections.Generic;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.UnitTests.Helpers.Builders;

internal class DocumentTemplateBuilder : ITestDataBuilder<DocumentTemplate>
{
    private DocumentTemplate _template = new();

    public DocumentTemplateBuilder WithId(int id)
    {
        _template.Id = id;
        return this;
    }

    public DocumentTemplateBuilder WithDefaultValues(DocumentType type)
    {
        _template = new DocumentTemplate()
        {
            Id = 1,
            Name = "Test Template",
            Abbreviation = "TEST",
            Description = $"Test {type} document template",
            NextNumber = 1,
            Prefix = "TST-",
            Postfix = string.Empty,
            DocumentType = type,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null
        };

        return this;
    }

    public DocumentTemplateBuilder WithDefaultPurchaseBillValues()
    {
        _template = new DocumentTemplate()
        {
            Id = 1,
            Name = "Purchase Bill",
            Abbreviation = "PB",
            Description = "Handles purchase of goods from suppliers",
            NextNumber = 1,
            Prefix = "PB-",
            Postfix = string.Empty,
            DocumentType = DocumentType.purchase_bill,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null
        };

        return this;
    }
    public DocumentTemplateBuilder WithDefaultSalesInvoiceValues()
    {
        _template = new DocumentTemplate()
        {
            Id = 1,
            Name = "Sales Invoice",
            Abbreviation = "SI",
            Description = "Handles sales of goods to clients",
            NextNumber = 1,
            Prefix = "SI-",
            Postfix = string.Empty,
            DocumentType = DocumentType.sales_invoice,
            DateCreated = DateTime.UtcNow,
            DateUpdated = null
        };

        return this;
    }


    public DocumentTemplate Build()
    {
        return _template;
    }

    public IEnumerable<DocumentTemplate> BuildDefaultList(int length, bool withIds)
    {
        throw new System.NotImplementedException();
    }

    ITestDataBuilder<DocumentTemplate> ITestDataBuilder<DocumentTemplate>.WithDefaultValues()
    {
        return WithDefaultValues(DocumentType.purchase_bill);
    }
}
