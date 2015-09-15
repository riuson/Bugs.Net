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
    internal class ChangeRepositoryTest
    {
        [Test]
        public virtual void CanSave()
        {
            long membersBefore = 0;
            long blobsBefore = 0;
            long changesBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Member author = new Member();

                var change = new Change();
                change.Author = author;
                change.Created = DateTime.Now;
                change.Description = new BlobContent();

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                changesBefore = changeRepository.RowCount();

                memberRepository.Save(author);
                changeRepository.Save(change);
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long changesAfter = changeRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore + 1));
                Assert.That(changesAfter, Is.EqualTo(changesBefore + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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
                id = change.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Change change2 = changeRepository.GetById(id);
                Member author2 = change2.Author;
                BlobContent description2 = change2.Description;

                Assert.That(change2.Author.FirstName, Is.EqualTo("First"));
                Assert.That(change2.Author.LastName, Is.EqualTo("Last"));
                Assert.That(change2.Author.EMail, Is.EqualTo("Email"));
                Assert.That(change2.Description.GetString(), Is.EqualTo("Description"));
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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
                id = change.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Change change = changeRepository.GetById(id);

                change.Description.SetString("Test");
                change.Created = new DateTime(2000, 11, 01);
                changeRepository.SaveOrUpdate(change);
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Change change = changeRepository.Load(id);

                Assert.That(change.Description.GetString(), Is.EqualTo("Test"));
                Assert.That(change.Created, Is.EqualTo(new DateTime(2000, 11, 01)));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;
            long membersBefore = 0;
            long blobsBefore = 0;
            long changesBefore = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
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

                membersBefore = memberRepository.RowCount();
                blobsBefore = blobRepository.RowCount();
                changesBefore = changeRepository.RowCount();

                memberRepository.Save(author);
                changeRepository.Save(change);
                id = change.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Change> changeRepository = new Repository<Change>(session);
                IRepository<Member> memberRepository = new Repository<Member>(session);
                IRepository<BlobContent> blobRepository = new Repository<BlobContent>(session);

                Change change = changeRepository.GetById(id);
                changeRepository.Delete(change);

                long membersAfter = memberRepository.RowCount();
                long blobsAfter = blobRepository.RowCount();
                long changesAfter = changeRepository.RowCount();

                Assert.That(membersAfter, Is.EqualTo(membersBefore + 1));
                Assert.That(blobsAfter, Is.EqualTo(blobsBefore));
                Assert.That(changesAfter, Is.EqualTo(changesBefore));
            }
        }
    }
}
