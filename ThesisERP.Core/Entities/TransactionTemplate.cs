using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities
{
    public class TransactionTemplate
    {
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
        public byte[] Timestamp { get; set; }
    }
}
