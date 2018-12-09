using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Address
    {
        public Address()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
        }

        public int AddressId { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public DateTime LastUpdate { get; set; }

        public ICollection<CustomerAddress> CustomerAddress { get; set; }
    }
}
