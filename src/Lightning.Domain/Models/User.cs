using System;
using System.Collections.Generic;

namespace Lightning.Domain.Models
{
    public class User
    {
        public virtual Guid ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}