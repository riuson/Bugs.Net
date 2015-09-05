﻿using BugTracker.Core.Interfaces;
using BugTracker.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Projects.Events
{
    public class LoginAnswerEventArgs : MessageEventArgs
    {
        public Member LoggedMember { get; private set; }

        public LoginAnswerEventArgs()
        {
            this.LoggedMember = null;
        }

        public LoginAnswerEventArgs(Member loggedMember)
        {
            this.LoggedMember = loggedMember;
        }
    }
}
