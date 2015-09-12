using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Ticket : Entity
    {
        public virtual string Title { get; set; }
        public virtual Member Author { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual Status Status { get; set; }
        public virtual ProblemType Type { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Solution Solution { get; set; }

        public virtual ICollection<Attachment> Attachments { get; protected set; }
        public virtual ICollection<Change> Changes { get; protected set; }

        public Ticket()
        {
            this.Attachments = new List<Attachment>();
            this.Changes = new List<Change>();
        }
    }
}
