using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models
{
    public class SuggestedProducts
    {
        public int ProductId { get; set; }
        public string ProductSource { get; set; }
        public string ProductName { get; set; }
        public int TotalPurchases { get; set; }
    }
}
