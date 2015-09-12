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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Dao
{
    internal class SessionManagerNH : ISessionManager
    {
        protected string mDatabaseFile;
        protected Exception mConfigurationException;

        protected Configuration Configuration { get; private set; }
        protected HbmMapping Mapping { get; private set; }
        protected ISessionFactory SessionFactory { get; private set; }

        public SessionManagerNH()
        {
            this.mDatabaseFile = "test.db"; // Saved<Options>.Instance.FileName;
            this.Configure();
        }

        protected bool Configure()
        {
            try
            {
                this.SessionFactory = this.BuildSessionFactory();
                this.IsConfigured = true;
            }
            catch (Exception exc)
            {
                this.mConfigurationException = exc;
                this.IsConfigured = false;
            }

            return this.IsConfigured;
        }

        protected ISessionFactory BuildSessionFactory()
        {
            this.Mapping = this.CreateMapping();
            this.Configuration = this.CreateConfiguration();
            return this.Configuration.BuildSessionFactory();
        }

        protected Configuration CreateConfiguration()
        {
            string connectioinString = String.Format("Data Source=\"{0}\";Version=3;New=True", this.mDatabaseFile);

            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Driver<SQLite20Driver>();
                db.ConnectionString = connectioinString;
                db.ConnectionStringName = "test";
                db.Dialect<SQLiteDialect>();
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
                db.AutoCommentSql = true;
            });
            configuration.AddAssembly(typeof(Priority).Assembly);
            configuration.AddDeserializedMapping(Mapping, null);

            return configuration;
        }

        protected HbmMapping CreateMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(new List<System.Type> {
                typeof(PriorityMap),
                typeof(ProblemTypeMap),
                typeof(SolutionMap),
                typeof(StatusMap)
            });
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        #region ISessionManager

        public BugTracker.DB.Interfaces.ISession OpenSession()
        {
            if (!this.IsConfigured)
            {
                if (!this.Configure())
                {
                    throw new Exception("Session factory not configured", this.mConfigurationException);
                }
            }

            NHibernate.ISession nhSession = this.SessionFactory.OpenSession();
            Session session = new Session(nhSession);
            return session;
        }

        public bool IsConfigured { get; private set; }

        #endregion
    }
}
