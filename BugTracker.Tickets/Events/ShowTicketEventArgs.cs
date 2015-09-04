using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Tickets.Events
{
    public class ShowTicketEventArgs : MessageEventArgs
    {
        public Ticket Item { get; private set; }

        public ShowTicketEventArgs(Ticket item)
        {
            this.Item = item;
        }
    }
}
