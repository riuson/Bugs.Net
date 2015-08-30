using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BugTracker.DB.Classes
{
    internal class Options
    {
        public string FileName { get; set; }

        public Options()
        {
            this.FileName = this.DefaultDatabaseFilename;
        }

        private string DefaultDatabaseFilename
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                path = Path.GetDirectoryName(path);
                return Path.Combine(path, "database.sqlite");
            }
        }
    }
}
