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
    internal class ChangeRepositoryTest
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
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();
                BlobContent description = new BlobContent();

                var change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = description;

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long changesBefore = changeRepository.RowCount();

                memberRepository.Save(author);
                changeRepository.Save(change);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long changesAfter = changeRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 1, blobsAfter);
                Assert.AreEqual(changesBefore + 1, changesAfter);
            }
        }

        [Test]
        public virtual void CanGet()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };
                BlobContent description = new BlobContent();
                description.SetString("Description");

                var change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = description;

                memberRepository.Save(author);
                changeRepository.Save(change);

                Change change2 = changeRepository.GetById(change.Id);
                Member author2 = change2.Author;
                BlobContent description2 = change2.Description;

                Assert.AreEqual(change2.Author.FirstName, "First");
                Assert.AreEqual(change2.Author.LastName, "Last");
                Assert.AreEqual(change2.Author.EMail, "Email");
                Assert.AreEqual(change2.Description.GetString(), "Description");
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };
                BlobContent description = new BlobContent();
                description.SetString("Description");

                var change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = description;

                memberRepository.Save(author);
                changeRepository.Save(change);

                Change change2 = changeRepository.GetById(change.Id);

                change2.Description.SetString("Test");
                changeRepository.SaveOrUpdate(change2);

                Change change3 = changeRepository.Load(change2.Id);

                Assert.AreEqual(change3.Description.GetString(), "Test");
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();
                BlobContent description = new BlobContent();

                var change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = description;

                long membersBefore = memberRepository.RowCount();
                long blobsBefore = blobRepository.RowCount();
                long changesBefore = changeRepository.RowCount();

                memberRepository.Save(author);
                changeRepository.Save(change);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long changesAfter = changeRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore + 1, blobsAfter);
                Assert.AreEqual(changesBefore + 1, changesAfter);

                changeRepository.Delete(change);

                membersAfter = memberRepository.RowCount();
                blobsAfter = blobRepository.RowCount();
                changesAfter = changeRepository.RowCount();

                Assert.AreEqual(membersBefore + 1, membersAfter);
                Assert.AreEqual(blobsBefore, blobsAfter);
                Assert.AreEqual(changesBefore, changesAfter);
            }
        }
    }
}
