using BugTracker.DB.Classes;
using BugTracker.DB.Tests.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Repositories
{
    [TestFixture]
    internal class AttachmentRepositoryTest
    {
        [SetUp]
        public virtual void Configure()
        {
            SessionManager.Instance.Configure("test.db");
            Assert.That(SessionManager.Instance.IsConfigured, Is.True);
        }

        [Test]
        public virtual void CanSave()
        {
            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();

                var attachment = new Attachment();
                attachment.Author = author;
                attachment.Created = DateTime.Now;
                attachment.File = new BlobContent();

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore + 1));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long id;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);

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
                id = attachment.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                Attachment attachment = attachmentRepository.GetById(id);

                Assert.That(attachment, Is.Not.Null);
                Assert.AreEqual(attachment.Author.FirstName, "First");
                Assert.AreEqual(attachment.Author.LastName, "Last");
                Assert.AreEqual(attachment.Author.EMail, "Email");
                Assert.AreEqual(attachment.File.GetString(), "Description");
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);

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
                id = attachment.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);

                Attachment attachment = attachmentRepository.GetById(id);

                attachment.File.SetString("Test");
                attachment.Comment = "comment2";
                attachment.Filename = "test2.txt";
                attachmentRepository.SaveOrUpdate(attachment);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);

                Attachment attachment = attachmentRepository.Load(id);

                Assert.That(attachment.File.GetString(), Is.EqualTo("Test"));
                Assert.That(attachment.Comment, Is.EqualTo("comment2"));
                Assert.That(attachment.Filename, Is.EqualTo("test2.txt"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;
            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;

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

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                attachmentsBefore = attachmentRepository.RowCount();

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);
                id = attachment.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Attachment attachment = attachmentRepository.GetById(id);
                attachmentRepository.Delete(attachment);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore));
            }
        }
    }
}
