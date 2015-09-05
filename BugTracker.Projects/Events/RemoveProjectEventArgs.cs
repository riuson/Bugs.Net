using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Projects.Events
{
    public class RemoveProjectEventArgs : MessageEventArgs
    {
        public Project Item { get; private set; }
        public Member LoggedMember { get; private set; }

        public RemoveProjectEventArgs(Project item, Member loggedMember)
        {
            this.Item = item;
            this.LoggedMember = loggedMember;
        }
    }
}
