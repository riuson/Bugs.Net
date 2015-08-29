﻿using FluentNHibernate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Models
{
    public class Project : Entity
    {
        public virtual string Name { get; set; }

        public virtual IList<Ticket> Tickets { get; protected set; }

        public Project()
        {
            this.Tickets = new List<Ticket>();
        }
    }
}
