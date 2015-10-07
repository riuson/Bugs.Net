using BugTracker.Core.Extensions;
using BugTracker.DB.Classes;
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
        private SessionOptions mOptions;

        public event EventHandler BeforeMigrate;
        public event EventHandler AfterMigrate;

        public Migrator(SessionOptions options)
        {
            this.mOptions = options;
        }

        public void Process()
        {
            ConfigurationLogDelegate log = this.mOptions.Log;

            if (String.IsNullOrEmpty(this.mOptions.Filename))
            {
                log("Filename not specified".Tr());
                return;
            }

            try
            {
                var parts = this.CollectMigrations();
                string connectionString = this.GetConnectionString(this.mOptions.Filename);

                FileInfo file = new FileInfo(this.mOptions.Filename);
                DirectoryInfo directory = file.Directory;

                if (!directory.Exists)
                {
                    directory.Create();
                }

                int currentVersion = 0;
                int latestVersion = 0;

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    currentVersion = this.GetCurrentVersion(connection);
                    latestVersion = this.GetLatestVersion(parts);
                    connection.Close();
                }

                log("Current version: ".Tr() + currentVersion);
                log("Latest version: ".Tr() + latestVersion);

                if (currentVersion > latestVersion)
                {
                    this.ThrowExceptionAboutVersion(currentVersion, latestVersion);
                }
                else if (currentVersion < latestVersion)
                {
                    if (this.BeforeMigrate != null)
                    {
                        this.BeforeMigrate(this, EventArgs.Empty);
                    }

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();
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

                    if (this.AfterMigrate != null)
                    {
                        this.AfterMigrate(this, EventArgs.Empty);
                    }
                }
                else
                {
                    log("Nothing to do".Tr());
                }
            }
            catch (Exception exc)
            {
                log(Environment.NewLine);
                log("Exception occured".Tr());
                log(exc.Message);
                log(exc.StackTrace);
                throw exc;
            }
        }

        public void ThrowExceptionAboutVersion(int currentVersion, int latestVersion)
        {
            throw new Exception(
                String.Format(
                    @"To avoid damaging the data, the application will not work with a database version (v.{0}) other than that supported (v.{1}) by the application.
Migration from newer to previous version is not supported.".Tr(),
                    currentVersion,
                    latestVersion));
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
            using (SQLiteCommand commandSetVersion = new SQLiteCommand(
                @"update BugTrackerInfo set Name = @name, Value = @value, Updated = @updated where Name = @name;
                  insert or ignore into BugTrackerInfo (Name, Value, Updated) values (@name, @value, @updated);", connection))
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

        private string GetConnectionString(string filename)
        {
            return String.Format("Data Source=\"{0}\";Version=3;", this.mOptions.Filename);
        }

        public int LatestVersion
        {
            get
            {
                var parts = this.CollectMigrations();
                return this.GetLatestVersion(parts);
            }
        }

        public int CurrentVersion
        {
            get
            {
                using (SQLiteConnection connection = new SQLiteConnection(this.GetConnectionString(this.mOptions.Filename)))
                {
                    connection.Open();

                    int currentVersion = this.GetCurrentVersion(connection);

                    connection.Close();

                    return currentVersion;
                }
            }
        }
    }
}
