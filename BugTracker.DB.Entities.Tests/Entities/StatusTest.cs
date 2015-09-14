using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class StatusTest
    {
        [Test]
        public void Operation()
        {
            var status = new Status
            {
                Value = "Test"
            };

            Assert.AreEqual(status.Value, "Test");
        }
    }
}
