using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.DataAccess.Repositories
{
    [TestFixture]
    internal class AttachmentRepositoryTest
    {
        [Test]
        public virtual void CanSave()
        {
            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
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

            using (ISession session = SessionManager.Instance.OpenSession(true))
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

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                Attachment attachment = attachmentRepository.GetById(id);

                Assert.That(attachment, Is.Not.Null);
                Assert.That(attachment.Author.FirstName, Is.EqualTo("First"));
                Assert.That(attachment.Author.LastName, Is.EqualTo("Last"));
                Assert.That(attachment.Author.EMail, Is.EqualTo("Email"));
                Assert.That(attachment.File.GetString(), Is.EqualTo("Description"));
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);

                Attachment attachment = attachmentRepository.GetById(id);

                attachment.File.SetString("Test");
                attachment.Comment = "comment2";
                attachment.Filename = "test2.txt";
                attachmentRepository.SaveOrUpdate(attachment);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
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

            using (ISession session = SessionManager.Instance.OpenSession(true))
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

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);

                Attachment attachment = attachmentRepository.GetById(id);
                attachmentRepository.Delete(attachment);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore));
            }
        }

        [Test]
        public virtual void CanKeepConstraintBlob()
        {
            long attachmentId = 0;
            long blobId = 0;
            long membersBefore = 0;
            long blobsBefore = 0;
            long attachmentsBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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
                attachmentId = attachment.Id;
                blobId = blob.Id;

                session.Transaction.Commit();
            }

            Assert.That(delegate()
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                        BlobContent blob = blobRepository.Load(blobId);
                        Assert.That(blob, Is.Not.Null);

                        blobRepository.Delete(blob);

                        long blobsAfter = blobRepository.RowCount();

                        session.Transaction.Commit();
                    }
                },
                Throws.Exception);

            Assert.That(delegate()
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                        BlobContent blob = blobRepository.GetById(blobId);
                        Assert.That(blob, Is.Not.Null);

                        blobRepository.Delete(blob);

                        //long blobsAfter = blobRepository.RowCount();
                        //Assert.That(blobsAfter, Is.EqualTo(blobsBefore));

                        session.Transaction.Commit();
                    }
                },
                Throws.Exception);


            using (ISession session = SessionManager.Instance.OpenSession(false))
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

                Attachment attachment = attachmentRepository.GetById(attachmentId);
                Assert.That(attachment, Is.Not.Null);

                BlobContent blob = attachment.File;
                Assert.That(blob, Is.Not.Null);
            }
        }

        [Test]
        public virtual void CanKeepConstraintAuthor()
        {
            long attachmentId = 0;
            long memberId = 0;
            long membersBefore = 0;
            long attachmentsBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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
                attachmentsBefore = attachmentRepository.RowCount();

                memberRepository.Save(author);
                attachmentRepository.Save(attachment);
                attachmentId = attachment.Id;
                memberId = author.Id;

                session.Transaction.Commit();
            }

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Member> memberRepository = new Repository<Member>(session);

                    Member member = memberRepository.Load(memberId);
                    Assert.That(member, Is.Not.Null);

                    memberRepository.Delete(member);

                    long membersAfter = memberRepository.RowCount();

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);

            Assert.That(delegate()
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Member> memberRepository = new Repository<Member>(session);

                    Member member = memberRepository.GetById(memberId);
                    Assert.That(member, Is.Not.Null);

                    memberRepository.Delete(member);

                    //long blobsAfter = blobRepository.RowCount();
                    //Assert.That(blobsAfter, Is.EqualTo(blobsBefore));

                    session.Transaction.Commit();
                }
            },
                Throws.Exception);


            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Attachment> attachmentRepository = new Repository<Attachment>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);

                long membersAfter = memberRepository.RowCount();
                long attachmentsAfter = attachmentRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(attachmentsAfter, Is.EqualTo(attachmentsBefore + 1));

                Attachment attachment = attachmentRepository.GetById(attachmentId);
                Assert.That(attachment, Is.Not.Null);

                BlobContent blob = attachment.File;
                Assert.That(blob, Is.Not.Null);
            }
        }
    }
}
