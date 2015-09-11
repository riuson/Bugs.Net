using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Events
{
    public abstract class EnityOperationEventArgs<T> : MessageEventArgs
    {
        public T Entity { get; private set; }
        public Member Member { get; private set; }

        public EnityOperationEventArgs()
        {
            this.Entity = default(T);
            this.Member = null;
        }

        public EnityOperationEventArgs(T entity)
        {
            this.Entity = entity;
            this.Member = null;
        }

        public EnityOperationEventArgs(T entity, Member member)
        {
            this.Entity = entity;
            this.Member = member;
        }

        public EnityOperationEventArgs(Member member)
        {
            this.Entity = default(T);
            this.Member = member;
        }
    }
}
