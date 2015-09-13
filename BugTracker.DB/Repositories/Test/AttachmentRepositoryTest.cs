using BugTracker.DB.Classes;
using BugTracker.DB.Dao.Test;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Repositories.Test
{
    [TestFixture]
    internal class AttachmentRepositoryTest
    {
        [SetUp]
        public virtual void Configure()
        {
            SessionManager.Instance.Configure("test.db");
            Assert.IsTrue(SessionManager.Instance.IsConfigured);
        }

        [Test]
        public virtual void CanSave()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();
                BlobContent blob = new BlobContent();

                var attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = blob;

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long attachmentsBefore = attachmentRepository.RowCount();

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 1, blobsAfter);
                Assert.AreEqual(attachmentsBefore + 1, attachmentsAfter);
            }
        }

        [Test]
        public virtual void CanGet()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };
                BlobContent blob = new BlobContent();
                blob.SetString("Description");

                var attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = blob;

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);

                Attachment attachment2 = attachmentRepository.GetById(attachment.Id);
                Member author2 = attachment2.Author;
                BlobContent blob2 = attachment2.File;

                Assert.AreEqual(attachment2.Author.FirstName, "First");
                Assert.AreEqual(attachment2.Author.LastName, "Last");
                Assert.AreEqual(attachment2.Author.EMail, "Email");
                Assert.AreEqual(attachment2.File.GetString(), "Description");
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };
                BlobContent blob = new BlobContent();
                blob.SetString("Description");

                var attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = blob;
                attachment.Comment = "comment";
                attachment.Filename = "test.txt";

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);

                Attachment attachment2 = attachmentRepository.GetById(attachment.Id);

                attachment2.File.SetString("Test");
                attachment2.Comment = "comment2";
                attachment2.Filename = "test2.txt";
                attachmentRepository.SaveOrUpdate(attachment2);

                Attachment attachment3 = attachmentRepository.Load(attachment2.Id);

                Assert.AreEqual(attachment3.File.GetString(), "Test");
                Assert.AreEqual(attachment3.Comment, "comment2");
                Assert.AreEqual(attachment3.Filename, "test2.txt");
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();
                BlobContent blob = new BlobContent();

                var attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = blob;

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long attachmentsBefore = attachmentRepository.RowCount();

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 1, blobsAfter);
                Assert.AreEqual(attachmentsBefore + 1, attachmentsAfter);

                attachmentRepository.Delete(attachment);

                membersAfter = memberRepository.RowCount();
                blobsAfter = blobRepository.RowCount();
                attachmentsAfter = attachmentRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore, blobsAfter);
                Assert.AreEqual(attachmentsBefore, attachmentsAfter);
            }
        }
    }
}
