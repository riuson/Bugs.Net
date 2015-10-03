using BugTracker.DB.Classes;
using BugTracker.DB.Entities;
using BugTracker.DB.Mapping;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BugTracker.DB.DataAccess
{
    internal static class SessionConfiguration
    {
        public static Configuration CreateConfiguration(SessionOptions options)
        {
            string connectioinString = String.Format("Data Source=\"{0}\";Version=3;New=True", options.Filename);

            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Driver<SQLite20Driver>();
                db.ConnectionString = connectioinString;
                db.ConnectionStringName = "bugtracker.database";
                db.Dialect<SQLiteDialect>();
                db.LogSqlInConsole = options.Log != null;
                db.LogFormattedSql = options.Log != null;
                db.AutoCommentSql = options.Log != null;
            });
            configuration.AddAssembly(typeof(Priority).Assembly);
            configuration.AddDeserializedMapping(CreateMapping(), null);

            return configuration;
        }

        public static IEnumerable<Type> GetEntityTypes()
        {
            Type entitybaseType = typeof(Entity<long>);

            var types = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where type != entitybaseType && entitybaseType.IsAssignableFrom(type)
                        orderby type.Name ascending
                        select type;

            var a = types.ToArray();

            return types;
        }

        public static HbmMapping CreateMapping()
        {
            var maps = from type in Assembly.GetExecutingAssembly().GetTypes()
                       where type.BaseType != null
                       where type.BaseType.IsGenericType
                       where type.BaseType.GetGenericTypeDefinition() == typeof(ClassMapping<>)
                       orderby type.Name ascending
                       select type;

            var mapper = new ModelMapper();
            mapper.AddMappings(maps);
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}
