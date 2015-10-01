using BugTracker.DB.DataAccess;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// Global setup/teardown methods for all tests in asssembly
/// </summary>
[SetUpFixture]
internal class TestDatabase
{
    [SetUp]
    public void Create()
    {
        File.Delete("test.db");
        // Configure database singleton for temp database file
        // before any tests
        SessionManager.Instance.Configure(new SessionOptions("test.db")
        {
            Log = this.Log,
            DoSchemaUpdate = true
        });
        Assert.That(SessionManager.Instance.IsConfigured, Is.True);
    }

    [TearDown]
    public void Remove()
    {
        // Remove temp database file after all tests completed
        //File.Delete("test.db");
    }

    private void Log(string message)
    {
        Console.WriteLine(message);
    }
}
