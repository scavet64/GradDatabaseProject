using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kinabalu.DAL;
using Newtonsoft.Json;

namespace Kinabalu.Models
{
    public class Customer
    {
        public Customer()
        {
            
        }

        public Customer(ApplicationDb db, int customerId, string firstName, string lastName, string emailAddress, DateTime lastLogin, DateTime lastUpdate)
        {
            CustomerId = customerId;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
            LastLogin = lastLogin;
            LastUpdate = lastUpdate;
        }

        public int CustomerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
