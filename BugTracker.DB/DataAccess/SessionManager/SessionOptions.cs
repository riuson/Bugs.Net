using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    public class SessionOptions
    {
        public string Filename { get; set; }
        public bool DoSchemaUpdate { get; set; }
        public ConfigurationLogDelegate Log { get; set; }

        public SessionOptions(string filename)
        {
            this.Filename = filename;
            this.Log = null;
            this.DoSchemaUpdate = false;
        }
    }
}
