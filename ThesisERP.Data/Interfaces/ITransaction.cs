using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data.Interfaces
{
    public interface ITransaction
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int TemplateId { get; set; }
        public int EntityId { get; set; }
        public Transactions.Status Status { get; set; }
        public ICollection<ITransactionDetail> Details { get; set; }
        public ICollection<IAddress> TransactionAddresses { get; set; }
    }

}
