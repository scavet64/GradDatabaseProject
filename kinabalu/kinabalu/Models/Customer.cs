using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdate { get; set; }

        public ICollection<CustomerAddress> CustomerAddress { get; set; }
    }
}
