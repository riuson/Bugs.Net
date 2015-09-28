using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Member : Entity<long>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string EMail { get; set; }

        public virtual string FullName
        {
            get
            {
                return String.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }
    }
}
