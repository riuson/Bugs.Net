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

        public SessionManagerInternal()
        {
            this.mDatabaseFile = Saved<Options>.Instance.FileName;
            this.Configure();
        }

        private bool Configure()
        {
            try
            {
                this.mSessionFactory = this.BuildSessionFactory();
                this.IsConfigured = true;
            }
            catch// (Exception exc)
            {
                this.IsConfigured = false;
            }

            return this.IsConfigured;
        }

        private ISessionFactory BuildSessionFactory()
        {
            AutoPersistenceModel model = this.CreateMappings();

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(this.mDatabaseFile))
                .Mappings(m => m.AutoMappings.Add(model))
                .ExposeConfiguration(this.BuildSchema)
                .BuildSessionFactory();
        }

        private AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .AssemblyOf<BugTracker.DB.Entities.IVocabulary>()
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
                if (!this.Configure())
                {
                    throw new Exception("Session factory not configured");
                }
            }

            NHibernate.ISession nhSession = this.mSessionFactory.OpenSession();
            Session session = new Session(nhSession);
            return session;
        }

        public bool IsConfigured { get; private set; }
    }
}
