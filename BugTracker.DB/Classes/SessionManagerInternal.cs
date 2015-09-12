using BugTracker.Core.Classes;
using BugTracker.DB.Interfaces;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Classes
{
    internal sealed class SessionManagerInternal : ISessionManager
    {
        private ISessionFactory mSessionFactory;
        private string mDatabaseFile;
        private Exception mConfigurationException;

        public SessionManagerInternal()
        {
            this.IsConfigured = false;
        }

        public bool Configure(string filename)
        {
            try
            {
                this.mSessionFactory = this.BuildSessionFactory(filename);
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

        private ISessionFactory BuildSessionFactory(string filename)
        {
            AutoPersistenceModel model = this.CreateMappings();

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(filename))
                .Mappings(m => m.AutoMappings.Add(model))
                .ExposeConfiguration(this.BuildSchema)
                .BuildSessionFactory();
        }

        private AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .AssemblyOf<BugTracker.DB.Entities.IVocabulary>()
                .Where(t => t.Namespace == typeof(BugTracker.DB.Entities.IVocabulary).Namespace)
                .IgnoreBase<BugTracker.DB.Entities.IVocabulary>();
        }

        private void BuildSchema(Configuration config)
        {
            // Use existing file
            if (File.Exists(this.mDatabaseFile))
            {
                new SchemaUpdate(config);
            }
            else // Create new file
            {
                new SchemaExport(config).Create(false, true);
            }
        }

        public BugTracker.DB.Interfaces.ISession OpenSession()
        {
            if (!this.IsConfigured)
            {
                throw new Exception("Session factory not configured", this.mConfigurationException);
            }

            NHibernate.ISession nhSession = this.mSessionFactory.OpenSession();
            Session session = new Session(nhSession);
            return session;
        }

        public bool IsConfigured { get; private set; }
    }
}
