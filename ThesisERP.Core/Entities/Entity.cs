using ThesisERP.Core.Entities;
using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public Enums.Entities.EntityTypes EntityType { get; set; }
        public string? Organization { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ICollection<Product>? RelatedProducts { get; set; }        

        public byte[] Timestamp { get; set; }

        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }

        public Entity(){ }

        public Entity(Enums.Entities.EntityTypes type, string firstName, string lastName, string email, string? organization = null)
        {
            this.EntityType = type;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Organization = organization;
            this.DateCreated = DateTime.Now;
            this.RelatedProducts = new List<Product>();            
        }

    }
}
