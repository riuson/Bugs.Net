using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities
{
    public class ProblemType : Entity, IVocabulary
    {
        public virtual string Value { get; set; }
    }
}
