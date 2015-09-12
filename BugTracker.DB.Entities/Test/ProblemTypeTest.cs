using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Test
{
    [TestFixture]
    internal class ProblemTypeTest
    {
        [Test]
        public void Operation()
        {
            var problemType = new ProblemType
            {
                Value = "Test"
            };

            Assert.AreEqual(problemType.Value, "Test");
        }
    }
}
