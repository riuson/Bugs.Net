using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Tickets.Events
{
    public class EditTicketEventArgs : MessageEventArgs
    {
        public Ticket Item { get; private set; }
        public Member LoggedMember { get; private set; }

        public EditTicketEventArgs(Ticket item, Member loggedMember)
        {
            this.Item = item;
            this.LoggedMember = loggedMember;
        }
    }
}
