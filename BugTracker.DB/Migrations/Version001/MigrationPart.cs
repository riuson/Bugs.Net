using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Migrations.Version001
{
    internal class MigrationPart : IMigrationPart
    {
        public int Version
        {
            get { return 1; }
        }

        public bool Upgrade(SQLiteConnection connection, MigrationLog log)
        {
            return true;
        }
    }
}
