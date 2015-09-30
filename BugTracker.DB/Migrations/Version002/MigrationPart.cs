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
        }

        private IEnumerable<String> GetPrepareCommands()
        {
            var result = @"
alter table Priority rename to Priority_;

alter table ProblemType rename to ProblemType_;

alter table Solution rename to Solution_;

alter table Status rename to Status_;

alter table BlobContent rename to BlobContent_;

alter table Member rename to Member_;

alter table Change rename to Change_;

alter table Attachment rename to Attachment_;

alter table Ticket rename to Ticket_;

alter table Project rename to Project_;


create table Priority (
    Id  integer primary key autoincrement,
    Value TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table ProblemType (
    Id  integer primary key autoincrement,
    Value TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table Solution (
    Id  integer primary key autoincrement,
    Value TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table Status (
    Id  integer primary key autoincrement,
    Value TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table BlobContent (
    Id  integer primary key autoincrement,
    Content BLOB,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table Member (
    Id  integer primary key autoincrement,
    FirstName TEXT,
    LastName TEXT,
    EMail TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table Change (
    Id  integer primary key autoincrement,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP,
    Member_Id BIGINT not null,
    BlobContent_Id BIGINT not null unique,
    Ticket_Id BIGINT,
    constraint FK67E23B1EFE153ED4 foreign key (Member_Id) references Member,
    constraint FK67E23B1ECB006E9C foreign key (BlobContent_Id) references BlobContent,
    constraint FK67E23B1E3A716DD6 foreign key (Ticket_Id) references Ticket);

create table Attachment (
    Id  integer primary key autoincrement,
    Filename TEXT,
    Member_Id BIGINT not null,
    Comment TEXT,
    BlobContent_Id BIGINT not null unique,
    Ticket_Id BIGINT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP,
    constraint FKA6AC5DDDFE153ED4 foreign key (Member_Id) references Member,
    constraint FKA6AC5DDDCB006E9C foreign key (BlobContent_Id) references BlobContent,
    constraint FKA6AC5DDD3A716DD6 foreign key (Ticket_Id) references Ticket);

create table Ticket (
    Id  integer primary key autoincrement,
    Project_Id BIGINT,
    Title TEXT,
    Member_Id BIGINT not null,
    Status_Id BIGINT not null,
    ProblemType_Id BIGINT not null,
    Priority_Id BIGINT not null,
    Solution_Id BIGINT not null,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP,
    constraint FKE14EC5362644F638 foreign key (Project_Id) references Project,
    constraint FKE14EC536FE153ED4 foreign key (Member_Id) references Member,
    constraint FKE14EC536EC9FD9F0 foreign key (Status_Id) references Status,
    constraint FKE14EC5368A1BCDC8 foreign key (ProblemType_Id) references ProblemType,
    constraint FKE14EC536380AADDC foreign key (Priority_Id) references Priority,
    constraint FKE14EC5364D19BABC foreign key (Solution_Id) references Solution);

create table Project (
    Id  integer primary key autoincrement,
    Name TEXT,
    Created DATETIME DEFAULT CURRENT_TIMESTAMP,
    Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

create table BugTrackerInfo (
   Id  integer primary key autoincrement,
   Name TEXT,
   Value TEXT,
   Created DATETIME DEFAULT CURRENT_TIMESTAMP,
   Updated DATETIME DEFAULT CURRENT_TIMESTAMP);

insert into Priority (Id, Value)
   select Id, Value
   from Priority_;

insert into ProblemType (Id, Value)
   select Id, Value
   from ProblemType_;

insert into Solution (Id, Value)
   select Id, Value
   from Solution_;

insert into Status (Id, Value)
   select Id, Value
   from Status_;

insert into BlobContent (Id, Content)
   select Id, Content
   from BlobContent_;

insert into Member (Id, FirstName, LastName, EMail)
   select Id, FirstName, LastName, EMail
   from Member_;

insert into Change (Id, Created, Member_Id, BlobContent_Id, Ticket_Id)
   select Id, Created, Member_Id, BlobContent_Id, Ticket_Id
   from Change_;

insert into Attachment (Id, Filename, Member_Id, Comment, BlobContent_Id, Ticket_Id, Created)
   select Id, Filename, Member_Id, Comment, BlobContent_Id, Ticket_Id, Created
   from Attachment_;

insert into Ticket (Id, Project_Id, Title, Member_Id, Status_Id, ProblemType_Id, Priority_Id, Solution_Id, Created)
   select Id, Project_Id, Title, Member_Id, Status_Id, ProblemType_Id, Priority_Id, Solution_Id, Created
   from Ticket_;

drop table Priority_;

drop table ProblemType_;

drop table Solution_;

drop table Status_;

drop table BlobContent_;

drop table Member_;

drop table Change_;

drop table Attachment_;

drop table Ticket_;

drop table Project_;
            ".Split(new string[] { "\r\r", "\n\n", "\r\n\r\n", "\n\r\n\r" }, StringSplitOptions.RemoveEmptyEntries);
            return result.Select(cmd => cmd.Trim());
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
            foreach (var commandText in this.GetPrepareCommands())
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
