using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class CustomerAddress
    {
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }

        public Address Address { get; set; }
        public Customer Customer { get; set; }
    }
}
