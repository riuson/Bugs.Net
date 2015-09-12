using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Dao.Test
{
    [TestFixture]
    internal class SchemaTest
    {
        private SessionManagerTest mSessionManager;

        [SetUp]
        public void CreateSessionManager()
        {
            this.mSessionManager = new SessionManagerTest();
        }

        [Test]
        public void CanGenerateSchema()
        {
            var schemaUpdate = new SchemaUpdate(this.mSessionManager.Configuration);
            schemaUpdate.Execute(Console.WriteLine, true);
        }

        private class SessionManagerTest : SessionManagerNH
        {
            public new Configuration Configuration
            {
                get
                {
                    return base.Configuration;
                }
            }
        }
    }
}
