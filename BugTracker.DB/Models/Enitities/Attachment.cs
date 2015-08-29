using FluentNHibernate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Models
{
    public class Attachment : Entity
    {
        public virtual string Filename { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual Member Author { get; set; }
        public virtual string Comment { get; set; }
        public virtual BlobContent File { get; set; }
    }
}
