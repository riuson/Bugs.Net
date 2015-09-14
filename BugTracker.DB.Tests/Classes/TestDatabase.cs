using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests
{
    [SetUpFixture]
    internal class TestDatabase
    {
        [SetUp]
        public void Create()
        {
            // Configure database singleton for temp database file
            // before any tests
            SessionManager.Instance.Configure("test.db");
            Assert.That(SessionManager.Instance.IsConfigured, Is.True);
        }

        [TearDown]
        public void Remove()
        {
            // Remove temp database file after all tests completed
            File.Delete("test.db");
        }
    }
}
