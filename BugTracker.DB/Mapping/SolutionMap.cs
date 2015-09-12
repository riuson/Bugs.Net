﻿using BugTracker.DB.Entities;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Mapping
{
    internal class SolutionMap : ClassMapping<Solution>
    {
        public SolutionMap()
        {
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.Value);
        }
    }
}
