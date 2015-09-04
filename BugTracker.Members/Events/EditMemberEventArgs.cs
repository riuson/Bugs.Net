using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Members.Events
{
    public class EditMemberEventArgs : MessageEventArgs
    {
        public Member Item { get; private set; }

        public EditMemberEventArgs(Member item)
        {
            this.Item = item;
        }
    }
}
