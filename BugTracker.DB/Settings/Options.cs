using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.DB.Settings
{
    public class Options
    {
        /// <summary>
        /// Path to database file
        /// </summary>
        public string DatabaseFileName { get; set; }
        /// <summary>
        /// The minimum period of the creation of backups, after which a new backup is created
        /// </summary>
        public int BackupKeepMinDays { get; set; }
        /// <summary>
        /// Maximum period to keep backup files, after which they are removed
        /// </summary>
        public int BackupKeepMaxDays { get; set; }
        /// <summary>
        /// Directory where backups saved
        /// </summary>
        public string BackupToDirectory { get; set; }

        public Options()
        {
            this.DatabaseFileName = this.DefaultDatabaseFilename;
            this.BackupKeepMinDays = 7;
            this.BackupKeepMaxDays = 60;

            string appdata = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.BackupToDirectory = Path.Combine(appdata, Application.CompanyName, Application.ProductName);
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
