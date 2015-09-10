using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.TicketEditor.Events
{
    internal class LoadAttachmentsEventArgs : EventArgs
    {
        public string[] Filenames { get; private set; }

        public LoadAttachmentsEventArgs(string[] filenames)
        {
            this.Filenames = filenames;
        }
    }
}
