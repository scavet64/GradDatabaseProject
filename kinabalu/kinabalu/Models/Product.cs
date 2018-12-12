using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Product
    {
        public Product()
        {
            Restock = new HashSet<Restock>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public double Cost { get; set; }
        public int ReorderLevel { get; set; }
        public string WeightUnitOfMeasure { get; set; }
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdate { get; set; }

        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<Restock> Restock { get; set; }
    }
}
