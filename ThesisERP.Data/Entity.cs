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
    [Table("Entities")]
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public Entities.EntityTypes EntityType { get; set; }
        public string? Organization { get; set; }
        public string FirstName { get; set; }        
        public string? LastName { get; set; }
        
        [Required]
        public string Email { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Product> RelatedProducts { get; set; }
        public virtual ICollection<EntityAddress> EntityAdresses { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public EntityAddress BillingAddress => EntityAdresses.FirstOrDefault(add=>add.AddressType == Addresses.AddressTypes.billiing);
        public EntityAddress ShippingAddress => EntityAdresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.shipping);

    }
}
