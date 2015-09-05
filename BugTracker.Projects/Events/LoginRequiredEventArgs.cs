using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Projects.Events
{
    public class LoginRequiredEventArgs : MessageEventArgs
    {
        public Member LoggedMember { get; set; }

        public LoginRequiredEventArgs()
        {
            this.LoggedMember = null;
        }
    }
}
