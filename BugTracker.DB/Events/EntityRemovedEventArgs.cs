using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityRemovedEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityRemovedEventArgs(T entity)
            : base(entity)
        {
        }
    }
}
