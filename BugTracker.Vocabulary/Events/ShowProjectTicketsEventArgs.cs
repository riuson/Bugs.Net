using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Vocabulary.Events
{
    public class ShowProjectTicketsEventArgs<T> : MessageEventArgs where T : new()
    {
        public T Item { get; private set; }

        public ShowProjectTicketsEventArgs(T item)
        {
            this.Item = item;
        }
    }
}
