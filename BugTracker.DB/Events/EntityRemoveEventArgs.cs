using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityRemoveEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityRemoveEventArgs(T entity)
            : base(entity)
        {
        }

        public EntityRemoveEventArgs(T entity, Member member)
            : base(entity, member)
        {
        }
    }
}
