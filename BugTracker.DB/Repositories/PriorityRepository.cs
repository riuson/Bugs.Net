using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Repositories
{
    public class PriorityRepository : Repository<Priority>
    {
        public PriorityRepository(ISession session) : base(session)
        {

        }
    }
}
