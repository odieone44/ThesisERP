﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Core.Entities;
using ThesisERP.Core.Extensions;

namespace ThesisERP.Application.DTOs.Documents;

public class CreateDocumentDetailDTO
{    
    public int ProductId { get; set; }        
    public decimal ProductQuantity { get; set; }    
    public decimal UnitPrice { get; set; }
    public int? DiscountID { get; set; }
    public int? TaxID { get; set; }
}

public class DocumentDetailDTO 
{
    public int ProductId { get; set; }
    public string ProductSKU { get; set; }
    public string ProductDescription { get; set; }
    public decimal ProductQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? DiscountID { get; set; }
    public string? DiscountName { get; set; }
    public int? TaxID { get; set; }
    public string? TaxName { get; set; }    
    public decimal LineTotalTax { get; set; } = decimal.Zero;
    public decimal LineTotalDiscount { get; set; } = decimal.Zero;
    public decimal LineTotalNet { get; set; } = decimal.Zero;
    public decimal LineTotalGross {get; set;} = decimal.Zero;
}
