using BugTracker.DB.Entities;
using BugTracker.DB.Mapping;
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
    internal static class SessionConfiguration
    {
        public static Configuration CreateConfiguration(string filename)
        {
            string connectioinString = String.Format("Data Source=\"{0}\";Version=3;New=True", filename);

            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Driver<SQLite20Driver>();
                db.ConnectionString = connectioinString;
                db.ConnectionStringName = "bugtracker.database";
                db.Dialect<SQLiteDialect>();
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
                db.AutoCommentSql = true;
            });
            configuration.AddAssembly(typeof(Priority).Assembly);
            configuration.AddDeserializedMapping(CreateMapping(), null);

            var schemaUpdate = new SchemaUpdate(configuration);
            schemaUpdate.Execute(Console.WriteLine, true);

            return configuration;
        }

        public static HbmMapping CreateMapping()
        {
            var mapper = new ModelMapper();
            mapper.AddMappings(new List<System.Type> {
                typeof(PriorityMap),
                typeof(ProblemTypeMap),
                typeof(SolutionMap),
                typeof(StatusMap),
                typeof(BlobContentMap),
                typeof(MemberMap),
                typeof(ChangeMap),
                typeof(AttachmentMap),
                typeof(TicketMap),
                typeof(ProjectMap)
            });
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}
