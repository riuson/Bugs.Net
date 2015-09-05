using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Tickets.Events
{
    public class AddTicketEventArgs : MessageEventArgs
    {
        public Member LoggedMember { get; private set; }

        public AddTicketEventArgs(Member loggedMember)
        {
            this.LoggedMember = loggedMember;
        }
    }
}
