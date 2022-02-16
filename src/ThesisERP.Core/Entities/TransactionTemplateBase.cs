﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities;

public abstract class TransactionTemplateBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Abbreviation { get; set; }
    public string Prefix { get; set; } = string.Empty;
    public string Postfix { get; set; } = string.Empty;
    public long NextNumber { get; set; } = 1;
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; } = false;
}
