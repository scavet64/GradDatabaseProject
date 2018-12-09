using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class OrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductSource { get; set; }
        public int Quantity { get; set; }
        public DateTime LastUpdate { get; set; }

        public Order Order { get; set; }
    }
}
