using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Info : Entity<long>
    {
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
    }
}
