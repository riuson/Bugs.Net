﻿using BugTracker.Core.Extensions;
using BugTracker.DB.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
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
            ConfigurationLogDelegate log = options.Log;

            if (String.IsNullOrEmpty(options.Filename))
            {
                log("Filename not specified".Tr());
                return;
            }

            try
            {
                var parts = this.CollectMigrations();
                string connectionString = String.Format("Data Source=\"{0}\";Version=3;", options.Filename);

                FileInfo file = new FileInfo(options.Filename);
                DirectoryInfo directory = file.Directory;

                if (!directory.Exists)
                {
                    directory.Create();
                }

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    int currentVersion = this.GetCurrentVersion(connection);
                    int latestVersion = this.GetLatestVersion(parts);

                    log("Current version: ".Tr() + currentVersion);
                    log("Latest version: ".Tr() + latestVersion);

                    parts = from part in parts
                            where part.Version > currentVersion
                            orderby part.Version ascending
                            select part;

                    foreach (var part in parts)
                    {
                        SQLiteTransaction transaction = connection.BeginTransaction();

                        log(Environment.NewLine);

                        try
                        {
                            log("Run migration to version: ".Tr() + part.Version);
                            part.Upgrade(connection, log);
                            this.SetCurrentVersion(connection, part.Version);
                            transaction.Commit();
                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                            throw exc;
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception exc)
            {
                log(Environment.NewLine);
                log("Exception occured".Tr());
                log(exc.Message);
                log(exc.StackTrace);
            }
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

        private int GetCurrentVersion(SQLiteConnection connection)
        {
            // Check for any table exists
            using (SQLiteCommand commandTablesCount = new SQLiteCommand("select count(*) from sqlite_master where type='table';", connection))
            {
                int tablesCount = Convert.ToInt32(commandTablesCount.ExecuteScalar());

                // If no table exists, database is empty, version 0
                if (tablesCount == 0)
                {
                    return 0;
                }
            }

            // Check for info table exists
            using (SQLiteCommand commandTableInfo = new SQLiteCommand("select count(*) from sqlite_master where type = 'table' and name = 'BugTrackerInfo';", connection))
            {
                int tablesCount = Convert.ToInt32(commandTableInfo.ExecuteScalar());

                // If no info table exists, version 1
                if (tablesCount == 0)
                {
                    return 1;
                }
            }

            // Get version from table
            using (SQLiteCommand commandGetVersion = new SQLiteCommand("select Value from BugTrackerInfo where Name = 'Version';", connection))
            {
                int version = Convert.ToInt32(commandGetVersion.ExecuteScalar());
                return version;
            }
        }

        private bool SetCurrentVersion(SQLiteConnection connection, int version)
        {
            // Check for any table exists
            using (SQLiteCommand commandTablesCount = new SQLiteCommand("select count(*) from sqlite_master where type='table';", connection))
            {
                int tablesCount = Convert.ToInt32(commandTablesCount.ExecuteScalar());

                // If no table exists, database is empty, version 0
                if (tablesCount == 0)
                {
                    return false;
                }
            }

            // Check for info table exists
            using (SQLiteCommand commandTableInfo = new SQLiteCommand("select count(*) from sqlite_master where type = 'table' and name = 'BugTrackerInfo';", connection))
            {
                int tablesCount = Convert.ToInt32(commandTableInfo.ExecuteScalar());

                // If no info table exists, version 1
                if (tablesCount == 0)
                {
                    return false;
                }
            }

            // Set version from table
            using (SQLiteCommand commandSetVersion = new SQLiteCommand("insert into BugTrackerInfo (Name, Value, Updated) values (@name, @value, @updated);", connection))
            {
                SQLiteParameter parameter = commandSetVersion.Parameters.Add("@name", System.Data.DbType.String);
                parameter.Value = "Version";

                parameter = commandSetVersion.Parameters.Add("@value", System.Data.DbType.String);
                parameter.Value = version.ToString();

                parameter = commandSetVersion.Parameters.Add("@updated", System.Data.DbType.DateTime);
                parameter.Value = DateTime.Now;

                int affectedRows = commandSetVersion.ExecuteNonQuery();

                System.Diagnostics.Debug.Assert(affectedRows == 1);
            }

            return true;
        }

        private int GetLatestVersion(IEnumerable<IMigrationPart> parts)
        {
            var result = (from part in parts
                          select part.Version).Max();

            return result;
        }
    }
}
