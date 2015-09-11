using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.TicketEditor.Events
{
    internal class SaveAttachmentEventArgs : EventArgs
    {
        public Attachment Attachment { get; private set; }
        public string Filename { get; private set; }

        public SaveAttachmentEventArgs(Attachment attachment, string filename)
        {
            this.Attachment = attachment;
            this.Filename = filename;
        }
    }
}
