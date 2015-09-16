using BugTracker.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Attachment : Entity<long>
    {
        public virtual string Filename { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual Member Author { get; set; }
        public virtual string Comment { get; set; }
        public virtual BlobContent File { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
