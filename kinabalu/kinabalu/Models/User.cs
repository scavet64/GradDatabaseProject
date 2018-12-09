using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerSource { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
