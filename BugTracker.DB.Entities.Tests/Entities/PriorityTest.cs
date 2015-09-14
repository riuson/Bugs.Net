using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class PriorityTest
    {
        [Test]
        public void Operation()
        {
            var priority = new Priority
            {
                Value = "Test"
            };

            Assert.AreEqual(priority.Value, "Test");
        }
    }
}
