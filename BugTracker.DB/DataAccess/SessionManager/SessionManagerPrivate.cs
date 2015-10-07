using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Mapping;
using BugTracker.DB.Settings;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    internal class SessionManagerPrivate : ISessionManager
    {
        protected Exception mConfigurationException;

        protected Configuration Configuration { get; private set; }
        protected ISessionFactory SessionFactory { get; private set; }

        public SessionManagerPrivate()
        {
        }

        public SessionManagerPrivate(SessionOptions sessionOptions)
        {
            this.Configure(sessionOptions);
        }

        public bool Configure(SessionOptions sessionOptions)
        {
            if (String.IsNullOrEmpty(sessionOptions.Filename))
            {
                return false;
            }

            if (sessionOptions.Log == null)
            {
                sessionOptions.Log = this.LogDebug;
            }

            try
            {
                Backup backup = new Backup(sessionOptions.Filename);
                backup.Process();

                Migrations.Migrator migrator = new Migrations.Migrator(sessionOptions);

                if (sessionOptions.DoSchemaUpdate)
                {
                    migrator.Process();
                }

                if (migrator.CurrentVersion != migrator.LatestVersion)
                {
                    migrator.ThrowExceptionAboutVersion(migrator.CurrentVersion, migrator.LatestVersion);
                }

                this.SessionFactory = this.BuildSessionFactory(sessionOptions);
                this.IsConfigured = true;
            }
            catch (Exception exc)
            {
                this.mConfigurationException = exc;
                this.IsConfigured = false;
            }

            return this.IsConfigured;
        }

        protected ISessionFactory BuildSessionFactory(SessionOptions sessionOptions)
        {
            this.Configuration = SessionConfiguration.CreateConfiguration(sessionOptions);
            return this.Configuration.BuildSessionFactory();
        }

        private void LogDebug(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        #region ISessionManager

        public BugTracker.DB.DataAccess.ISession OpenSession(bool beginTransaction)
        {
            if (!this.IsConfigured)
            {
                this.Configure(new SessionOptions(Saved<Options>.Instance.DatabaseFileName));
            }

            if (!this.IsConfigured)
            {
                throw new Exception("Session factory not configured", this.mConfigurationException);
            }

            NHibernate.ISession nhSession = this.SessionFactory.OpenSession();
            Session session = new Session(nhSession, beginTransaction);
            return session;
        }

        public bool IsConfigured { get; private set; }

        public bool TestConfiguration()
        {
            if (this.IsConfigured)
            {
                return true;
            }

            this.Configure(new SessionOptions(Saved<Options>.Instance.DatabaseFileName));
            return this.IsConfigured;
        }

        #endregion
    }
}
