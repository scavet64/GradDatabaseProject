using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models
{
    public class BelowMinimumStockView
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public int Difference { get; set; }
    }
}
