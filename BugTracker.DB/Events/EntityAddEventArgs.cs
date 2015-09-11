using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityAddEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityAddEventArgs()
            : base()
        {

        }

        public EntityAddEventArgs(Member member)
            : base(member)
        {

        }
    }
}
