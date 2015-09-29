using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Migrations.Version002
{
    internal class MigrationPart : IMigrationPart
    {
        private IEnumerable<String> mPrepareCommands;

        public MigrationPart()
        {
            this.mPrepareCommands = new string[]
            {
                @"create table BugTrackerInfo (
                    Id  integer primary key autoincrement,
                    Name TEXT,
                    Value TEXT,
                    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
                    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);"
            };
        }

        public int Version
        {
            get { return 2; }
        }

        public bool Upgrade(SQLiteConnection connection, MigrationLog log)
        {
            bool result = true;

            result &= this.Prepare(connection, log);

            if (result)
            {
                result &= this.Complete(connection, log);
            }

            return result;
        }

        private bool Prepare(SQLiteConnection connection, MigrationLog log)
        {
            foreach (var commandText in this.mPrepareCommands)
            {
                using (SQLiteCommand command = new SQLiteCommand(commandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            return true;
        }

        private bool Complete(SQLiteConnection connection, MigrationLog log)
        {
            return true;
        }
    }
}
