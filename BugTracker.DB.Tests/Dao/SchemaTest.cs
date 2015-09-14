using BugTracker.DB.Dao;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Dao
{
    [TestFixture]
    internal class SchemaTest
    {
        [Test]
        public void CanGenerateSchema()
        {
            string filename = "test_schema.db";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            Configuration configuration = SessionConfiguration.CreateConfiguration(filename);
            var schemaUpdate = new SchemaUpdate(configuration);
            schemaUpdate.Execute(Console.WriteLine, true);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}
