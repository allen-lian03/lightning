using System;
using System.Collections.Generic;

namespace Lightning.Domain.Models
{
    public class Role
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
    }
}