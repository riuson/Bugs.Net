using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityAddedEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityAddedEventArgs(T entity)
            : base(entity)
        {
        }
    }
}
