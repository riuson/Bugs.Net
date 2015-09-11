using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public class EntityShowEventArgs<T> : EnityOperationEventArgs<T>
    {
        public EntityShowEventArgs(T entity)
            : base(entity)
        {
        }

        public EntityShowEventArgs(Member member)
            : base(member)
        {
        }

        public EntityShowEventArgs()
            : base()
        {
        }

        public EntityShowEventArgs(T entity, Member member)
            : base(entity, member)
        {
        }
    }
}
