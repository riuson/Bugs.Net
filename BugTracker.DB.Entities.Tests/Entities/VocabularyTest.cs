using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture(typeof(Priority))]
    [TestFixture(typeof(ProblemType))]
    [TestFixture(typeof(Solution))]
    [TestFixture(typeof(Status))]
    internal class VocabularyTest<T> where T : DB.Entities.Entity<long>, IVocabulary, new()
    {
        [Test]
        public void Operation()
        {
            var status = new Status
            {
                Value = "Test"
            };

            Assert.That(status.Value, Is.EqualTo("Test"));
        }
    }
}
