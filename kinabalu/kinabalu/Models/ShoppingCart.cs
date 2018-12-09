using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class ShoppingCart
    {
        public int CustomerId { get; set; }
        public string CustomerSource { get; set; }
        public int ProductId { get; set; }
        public string ProductSource { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
