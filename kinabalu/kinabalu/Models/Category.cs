using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Category
    {
        public Category()
        {
            Product = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
