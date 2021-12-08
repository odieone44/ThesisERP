using System;
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
