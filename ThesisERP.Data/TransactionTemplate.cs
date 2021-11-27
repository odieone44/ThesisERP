using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data
{
    [Table("TransactionTemplates")]
    public class TransactionTemplate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        //public int NextNumber { get; set; } = 1;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Transactions.Types TransactionType { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
