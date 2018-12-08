using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Restock
    {
        public int RestockId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public byte Fulfilled { get; set; }
        public DateTime LastUpdate { get; set; }

        public Product Product { get; set; }
    }
}
