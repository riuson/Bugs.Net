using BugTracker.Core.Classes;
using BugTracker.DB.Classes;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.DB
{
    /// <summary>
    /// Database class
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Creator
        /// </summary>
        private sealed class DatabaseCreator
        {
            /// <summary>
            /// Lazy initializer
            /// </summary>
            public static Database Instance
            {
                get { return mInstance; }
            }
            static readonly Database mInstance = new Database();
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Database()
        {
            this.mDatabaseFile = Saved<Options>.Instance.FileName;
            this.SessionFactory = this.BuildSessionFactory();
        }

        /// <summary>
        /// Singletone instance of Database object
        /// </summary>
        public static Database Instance
        {
            get { return DatabaseCreator.Instance; }
        }

        public ISessionFactory SessionFactory { get; private set; }

        private string mDatabaseFile;

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
                .Assembly(System.Reflection.Assembly.GetCallingAssembly())
                .Where(t => t.Namespace == "BugTracker.DB.Models");
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
    }
}
