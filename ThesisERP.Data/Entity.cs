using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisERP.Static.Enums;

namespace ThesisERP.Data
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public Entities.EntityType EntityType { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<EntityAddress> AddressList { get; set; }
        public EntityAddress BillingAddress => AddressList.FirstOrDefault(add=>add.AddressType == Addresses.AddressType.billiing);
        public EntityAddress ShippingAddress => AddressList.FirstOrDefault(add => add.AddressType == Addresses.AddressType.shipping);

    }
}
