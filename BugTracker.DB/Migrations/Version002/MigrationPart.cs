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
                @"alter table Attachment
                    add column Updated DATETIME;",

                @"alter table BlobContent
                    add column Created DATETIME;",

                @"alter table BlobContent
                    add column Updated DATETIME;",

                @"alter table Change
                    add column Updated DATETIME;",

                @"alter table Member
                    add column Created DATETIME;",

                @"alter table Member
                    add column Updated DATETIME;",

                @"alter table Priority
                    add column Created DATETIME;",

                @"alter table Priority
                    add column Updated DATETIME;",

                @"alter table ProblemType
                    add column Created DATETIME;",

                @"alter table ProblemType
                    add column Updated DATETIME;",

                @"alter table Project
                    add column Created DATETIME;",

                @"alter table Project
                    add column Updated DATETIME;",

                @"alter table Solution
                    add column Created DATETIME;",

                @"alter table Solution
                    add column Updated DATETIME;",

                @"alter table Status
                    add column Created DATETIME;",

                @"alter table Status
                    add column Updated DATETIME;",

                @"alter table Ticket
                    add column Updated DATETIME;",

                @"create table BugTrackerInfo (
                    Id  integer primary key autoincrement,
                    Name TEXT,
                    Value TEXT,
                    Created DATETIME,
                    Updated DATETIME);"
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
