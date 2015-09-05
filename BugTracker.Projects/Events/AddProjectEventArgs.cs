using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Projects.Events
{
    public class AddProjectEventArgs : MessageEventArgs
    {
        public Member LoggedMember { get; private set; }

        public AddProjectEventArgs(Member loggedMember)
        {
            this.LoggedMember = loggedMember;
        }
    }
}
