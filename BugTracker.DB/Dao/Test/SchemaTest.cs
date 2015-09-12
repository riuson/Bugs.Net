using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Dao.Test
{
    [TestFixture]
    internal class SchemaTest
    {
        [Test]
        public void CanGenerateSchema()
        {
            string filename = "test.db";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            Configuration configuration = SessionConfiguration.CreateConfiguration("test.db");
            var schemaUpdate = new SchemaUpdate(configuration);
            schemaUpdate.Execute(Console.WriteLine, true);
        }
    }
}
