using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinabalu.Models.Views
{
    public class InactiveUserView
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime LastLogin { get; set; }
        public int DaysInactive { get; set; }
    }
}
