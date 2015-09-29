using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Migrations.Version001
{
    internal class MigrationPart : IMigrationPart
    {
        private IEnumerable<String> mPrepareCommands;

        public MigrationPart()
        {
            this.mPrepareCommands = new string[]
            {
                @"create table Priority (
                    Id  integer primary key autoincrement,
                    Value TEXT);",

                @"create table ProblemType (
                    Id  integer primary key autoincrement,
                    Value TEXT);",

                @"create table Solution (
                    Id  integer primary key autoincrement,
                    Value TEXT);",

                @"create table Status (
                    Id  integer primary key autoincrement,
                    Value TEXT);",

                @"create table BlobContent (
                    Id  integer primary key autoincrement,
                    Content BLOB);",

                @"create table Member (
                    Id  integer primary key autoincrement,
                    FirstName TEXT,
                    LastName TEXT,
                    EMail TEXT);",

                @"create table Change (
                    Id  integer primary key autoincrement,
                    Member_Id BIGINT not null,
                    Created DATETIME,
                    BlobContent_Id BIGINT not null unique,
                    Ticket_Id BIGINT,
                    constraint FK67E23B1EFE153ED4 foreign key (Member_Id) references Member,
                    constraint FK67E23B1ECB006E9C foreign key (BlobContent_Id) references BlobContent,
                    constraint FK67E23B1E3A716DD6 foreign key (Ticket_Id) references Ticket);",

                @"create table Attachment (
                    Id  integer primary key autoincrement,
                    Filename TEXT,
                    Created DATETIME,
                    Member_Id BIGINT not null,
                    Comment TEXT,
                    BlobContent_Id BIGINT not null unique,
                    Ticket_Id BIGINT,
                    constraint FKA6AC5DDDFE153ED4 foreign key (Member_Id) references Member,
                    constraint FKA6AC5DDDCB006E9C foreign key (BlobContent_Id) references BlobContent,
                    constraint FKA6AC5DDD3A716DD6 foreign key (Ticket_Id) references Ticket);",

                @"create table Ticket (
                    Id  integer primary key autoincrement,
                    Project_Id BIGINT,
                    Title TEXT,
                    Member_Id BIGINT not null,
                    Created DATETIME,
                    Status_Id BIGINT not null,
                    ProblemType_Id BIGINT not null,
                    Priority_Id BIGINT not null,
                    Solution_Id BIGINT not null,
                    constraint FKE14EC5362644F638 foreign key (Project_Id) references Project,
                    constraint FKE14EC536FE153ED4 foreign key (Member_Id) references Member,
                    constraint FKE14EC536EC9FD9F0 foreign key (Status_Id) references Status,
                    constraint FKE14EC5368A1BCDC8 foreign key (ProblemType_Id) references ProblemType,
                    constraint FKE14EC536380AADDC foreign key (Priority_Id) references Priority,
                    constraint FKE14EC5364D19BABC foreign key (Solution_Id) references Solution);",

                @"create table Project (
                    Id  integer primary key autoincrement,
                    Name TEXT);"
            };
        }

        public int Version
        {
            get { return 1; }
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
