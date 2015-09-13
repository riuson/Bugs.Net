using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Test
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

            Assert.AreEqual(attachment.Author, author);
            Assert.AreEqual(attachment.Created, now);
            Assert.AreEqual(attachment.File, blob);
            Assert.AreEqual(attachment.Filename, "test.txt");
            Assert.AreEqual(attachment.Comment, "comment");
        }
    }
}
