using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Product = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public DateTime LastUpdate { get; set; }

        public Address Address { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
