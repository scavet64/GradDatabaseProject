using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models
{
    public class ProductsView
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public double Cost { get; set; }
        public int? Quantity { get; set; }
        public string Source { get; set; }
        public double? AverageRating { get; set; }
        public double? AverageReceivedRating { get; set; }
    }
}
