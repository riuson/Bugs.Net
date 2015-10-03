using BugTracker.DB.DataAccess;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.DataAcces
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

            Configuration configuration = SessionConfiguration.CreateConfiguration(new SessionOptions(filename)
            {
                Log = this.Log
            });
            var schemaUpdate = new SchemaUpdate(configuration);
            schemaUpdate.Execute(Console.WriteLine, true);

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
