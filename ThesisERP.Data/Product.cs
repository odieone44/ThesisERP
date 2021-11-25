using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public Products.Types Type { get; set; }
        [Required]
        public string SKU { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public decimal DefaultPurchasePrice { get; set; }
        public decimal DefaultSaleSPrice { get; set; }
        public virtual ICollection<Entity> RelatedEntities { get; set; }

    }
}
