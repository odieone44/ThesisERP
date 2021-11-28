using ThesisERP.Static.Enums;

namespace ThesisERP.Core.Interfaces
{
    public interface ITransaction
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int TemplateId { get; set; }
        public int EntityId { get; set; }
        public Transactions.Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public ICollection<ITransactionDetail> Details { get; set; }
        public ICollection<IAddress> TransactionAddresses { get; set; }
    }

}
