using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Application.DTOs.Documents;

public class DocumentTemplateDTO : CreateDocumentTemplateDTO
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
}

public class CreateDocumentTemplateDTO
{
    [Required]
    [StringLength(40, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Name { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Description { get; set; }

    [Required]
    [StringLength(DocumentTemplate.AbbreviationMaxLength, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Abbreviation { get; set; }

    [StringLength(10)]
    public string Prefix { get; set; }

    [StringLength(10)]
    public string Postfix { get; set; }
    public long NextNumber { get; set; } = 1;

    [Required]
    public DocumentType DocumentType { get; set; }
}
