using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using BugTracker.DB.Mapping;
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

namespace BugTracker.DB.Dao
{
    internal class SessionManagerPrivate : ISessionManager
    {
        protected string mDatabaseFile;
        protected Exception mConfigurationException;

        protected Configuration Configuration { get; private set; }
        protected ISessionFactory SessionFactory { get; private set; }

        public SessionManagerPrivate()
        {
            this.Configure(this.mDatabaseFile);
        }

        public SessionManagerPrivate(string filename)
        {
            this.Configure(filename);
        }

        public bool Configure(string filename)
        {
            try
            {
                this.SessionFactory = this.BuildSessionFactory(filename);
                this.mDatabaseFile = filename;
                this.IsConfigured = true;
            }
            catch (Exception exc)
            {
                this.mConfigurationException = exc;
                this.IsConfigured = false;
            }

            return this.IsConfigured;
        }

        protected ISessionFactory BuildSessionFactory(string filename)
        {
            this.Configuration = SessionConfiguration.CreateConfiguration(filename);
            return this.Configuration.BuildSessionFactory();
        }

        #region ISessionManager

        public BugTracker.DB.Interfaces.ISession OpenSession(bool beginTransaction)
        {
            if (!this.IsConfigured)
            {
                throw new Exception("Session factory not configured", this.mConfigurationException);
            }

            NHibernate.ISession nhSession = this.SessionFactory.OpenSession();
            Session session = new Session(nhSession, beginTransaction);
            return session;
        }

        public bool IsConfigured { get; private set; }

        #endregion
    }
}
