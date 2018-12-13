using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.Views
{
    public class ProductLowSalesView
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Category { get; set; }
        public double Cost { get; set; }
        public int Sales { get; set; }
    }
}
