﻿using ThesisERP.Core.Enums;

namespace ThesisERP.Core.Entites
{
    public class Product
    {
        public int Id { get; set; }
        public Products.Types Type { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public string? LongDescription { get; set; }
        public decimal? DefaultPurchasePrice { get; set; }
        public decimal? DefaultSaleSPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public byte[] Timestamp { get; set; }
        public virtual ICollection<Entity> RelatedEntities { get; set; }

    }
}