using System.ComponentModel.DataAnnotations;
using ThesisERP.Core.Entities;

namespace ThesisERP.Application.DTOs.Transactions;

public abstract class TransactionTemplateBaseDTO
{
    public int Id { get; set; }

    [StringLength(40, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Name { get; set; }

    [StringLength(200, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Description { get; set; }

    [StringLength(TransactionTemplateBase.AbbreviationMaxLength, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Abbreviation { get; set; }

    [StringLength(10)]
    public string Prefix { get; set; }

    [StringLength(10)]
    public string Postfix { get; set; }
    public long NextNumber { get; set; } = 1;
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
}

public abstract class CreateTransactionTemplateBaseDTO
{
    [Required]
    [StringLength(40, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Name { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Description { get; set; }

    [Required]
    [StringLength(TransactionTemplateBase.AbbreviationMaxLength, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Abbreviation { get; set; }

    [StringLength(10)]
    public string Prefix { get; set; }

    [StringLength(10)]
    public string Postfix { get; set; }
    public long NextNumber { get; set; } = 1;
}

public abstract class UpdateTransactionTemplateBaseDTO
{
    [Required]
    [StringLength(40, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Name { get; set; }
    [Required]
    [StringLength(200, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Description { get; set; }

    [Required]
    [StringLength(TransactionTemplateBase.AbbreviationMaxLength, ErrorMessage = "Length must be between {2} and {1} characters.", MinimumLength = 1)]
    public string Abbreviation { get; set; }

    [StringLength(10)]
    public string? Prefix { get; set; } = string.Empty;

    [StringLength(10)]
    public string? Postfix { get; set; } = string.Empty;
}