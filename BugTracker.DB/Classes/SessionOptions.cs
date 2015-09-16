using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    public class SessionOptions
    {
        public string Filename { get; set; }
        public bool ShowLogs { get; set; }
        public bool DoSchemaUpdate { get; set; }

        public SessionOptions(string filename)
        {
            this.Filename = filename;
            this.ShowLogs = false;
            this.DoSchemaUpdate = true;
        }
    }
}
