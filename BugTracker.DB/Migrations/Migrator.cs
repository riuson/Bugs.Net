using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BugTracker.DB.Migrations
{
    internal class Migrator
    {
        public Migrator()
        {
        }

        public void Process(BugTracker.DB.DataAccess.SessionOptions options)
        {
            string filename = options.Filename;
            bool showLogs = options.ShowLogs;

            var parts = this.CollectMigrations();
        }

        private IEnumerable<IMigrationPart> CollectMigrations()
        {
            Type itype = typeof(IMigrationPart);

            var parts = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where itype.IsAssignableFrom(t) && itype != t
                        let part = Activator.CreateInstance(t) as IMigrationPart
                        orderby part.Version ascending
                        select part;

            return parts;
        }
    }
}
