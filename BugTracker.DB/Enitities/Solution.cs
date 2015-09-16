using BugTracker.DB.Classes;
using BugTracker.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class Solution : Entity<long>, IVocabulary
    {
        public virtual string Value { get; set; }
    }
}
