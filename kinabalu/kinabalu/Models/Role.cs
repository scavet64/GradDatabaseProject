using System;
using System.Collections.Generic;

namespace Kinabalu.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Role1 { get; set; }

        public ICollection<User> User { get; set; }
    }
}
