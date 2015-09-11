using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityEditEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityEditEventArgs(T entity)
            : base(entity)
        {
        }

        public EntityEditEventArgs(T entity, Member member)
            : base(entity, member)
        {
        }
    }
}
