using ThesisERP.Static.Enums;

namespace ThesisERP.Core.Entites
{
    public class Entity
    {
        public int Id { get; set; }
        public Entities.EntityTypes EntityType { get; set; }
        public string? Organization { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Product> RelatedProducts { get; set; }
        public virtual ICollection<EntityAddress> EntityAdresses { get; set; }

        public byte[] Timestamp { get; set; }

        public EntityAddress BillingAddress => EntityAdresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.billiing);
        public EntityAddress ShippingAddress => EntityAdresses.FirstOrDefault(add => add.AddressType == Addresses.AddressTypes.shipping);

    }
}
