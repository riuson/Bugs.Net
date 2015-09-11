using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityEditedEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityEditedEventArgs()
            : base()
        {
        }

        public EntityEditedEventArgs(T entity)
            : base(entity)
        {
        }
    }
}
