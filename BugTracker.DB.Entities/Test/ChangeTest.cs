using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Test
{
    [TestFixture]
    internal class ChangeTest
    {
        [Test]
        public void Operation()
        {
            Member author = new Member();
            BlobContent blob = new BlobContent();
            DateTime now =DateTime.Now;

            var change = new Change
            {
                Author = author,
                Created = now,
                Description = blob
            };

            Assert.AreEqual(change.Author, author);
            Assert.AreEqual(change.Created, now);
            Assert.AreEqual(change.Description, blob);
        }
    }
}
