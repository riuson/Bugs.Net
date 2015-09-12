using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Test
{
    [TestFixture]
    internal class SolutionTest
    {
        [Test]
        public void Operation()
        {
            var solution = new Solution
            {
                Value = "Test"
            };

            Assert.AreEqual(solution.Value, "Test");
        }
    }
}
