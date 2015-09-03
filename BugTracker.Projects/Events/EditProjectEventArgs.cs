using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Projects.Events
{
    public class EditProjectEventArgs : MessageEventArgs
    {
        public Project Item { get; private set; }

        public EditProjectEventArgs(Project item)
        {
            this.Item = item;
        }
    }
}
