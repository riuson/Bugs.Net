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
using BugTracker.DB.Dao;

namespace BugTracker.DB.Tests.Repositories
{
    [TestFixture]
    internal class BlobContentRepositoryTest
    {
        private Random mRandom;

        [SetUp]
        public virtual void Configure()
        {
            this.mRandom = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
        }

        [Test]
        public virtual void CanSave(
            [Values(1, 10, 100, 10000, 1000000)] int size)
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = this.GetRandomArray(size);
                before = repository.RowCount();
                repository.Save(x);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);

                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public virtual void CanGet(
            [Values(1, 10, 100, 10000, 1000000)] int size)
        {
            long id = 0;
            byte[] buffer1 = this.GetRandomArray(size);

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = buffer1;
                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                Assert.That(y, Is.Not.Null);
                byte[] buffer2 = y.Content;
                Assert.That(buffer1, Is.EqualTo(buffer2));
            }
        }

        [Test]
        public virtual void CanUpdate(
            [Values(1, 10, 100, 10000, 1000000)] int size)
        {
            long id = 0;
            byte[] buffer1 = this.GetRandomArray(size);

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = buffer1;
                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                buffer1 = this.GetRandomArray(size);
                y.Content = buffer1;
                repository.SaveOrUpdate(y);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                byte[] buffer2 = y.Content;
                Assert.That(buffer2, Is.EqualTo(buffer1));
            }
        }

        [Test]
        public virtual void CanDelete(
            [Values(1, 10, 100, 10000, 1000000)] int size)
        {
            long before = 0;
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = this.GetRandomArray(size);
                before = repository.RowCount();
                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                repository.Delete(y);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before));

                session.Transaction.Commit();
            }
        }

        private byte[] GetRandomArray(int size)
        {
            byte[] buffer = new byte[size];
            this.mRandom.NextBytes(buffer);
            return buffer;
        }
    }
}
