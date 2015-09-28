using BugTracker.DB.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Entities
{
    [TestFixture]
    internal class AttachmentTest
    {
        [Test]
        public void Operation()
        {
            Member author = new Member();
            BlobContent blob = new BlobContent();
            DateTime now = DateTime.Now;

            var attachment = new Attachment
            {
                Author = author,
                Created = now,
                File = blob,
                Filename = "test.txt",
                Comment = "comment"
            };

            Assert.That(attachment.Author, Is.EqualTo(author));
            Assert.That(attachment.Created, Is.EqualTo(now));
            Assert.That(attachment.File, Is.EqualTo(blob));
            Assert.That(attachment.Filename, Is.EqualTo("test.txt"));
            Assert.That(attachment.Comment, Is.EqualTo("comment"));
        }
    }
}
