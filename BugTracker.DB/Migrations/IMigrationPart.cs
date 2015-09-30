using BugTracker.DB.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Migrations
{
    internal interface IMigrationPart
    {
        int Version { get; }
        void Upgrade(SQLiteConnection connection, ConfigurationLogDelegate log);
    }
}
