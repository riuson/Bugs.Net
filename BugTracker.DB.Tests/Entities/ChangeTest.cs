using BugTracker.DB.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Entities
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

            Assert.That(change.Author, Is.EqualTo(author));
            Assert.That(change.Created, Is.EqualTo(now));
            Assert.That(change.Description, Is.EqualTo(blob));
        }
    }
}
