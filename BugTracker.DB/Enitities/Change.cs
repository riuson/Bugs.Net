﻿using BugTracker.DB.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Change : Entity<long>
    {
        public virtual Member Author { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual BlobContent Description { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
