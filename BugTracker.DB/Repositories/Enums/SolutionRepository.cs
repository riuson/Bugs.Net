using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Repositories
{
    public class SolutionRepository : Repository<Solution>
    {
        public SolutionRepository(ISession session) : base(session)
        {

        }
    }
}
