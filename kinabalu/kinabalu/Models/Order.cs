using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProduct = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerSource { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime LastUpdate { get; set; }

        public ICollection<OrderProduct> OrderProduct { get; set; }
    }
}
