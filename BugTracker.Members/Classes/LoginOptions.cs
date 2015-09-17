using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.Members.Classes
{
    public class LoginOptions
    {
        public long MemberId { get; set; }

        public LoginOptions()
        {
            this.MemberId = 0;
        }
    }
}
