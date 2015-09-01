﻿using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Repositories
{
    public class ProjectRepository : Repository<Project>
    {
        public ProjectRepository(ISession session)
            : base(session)
        {

        }
    }
}