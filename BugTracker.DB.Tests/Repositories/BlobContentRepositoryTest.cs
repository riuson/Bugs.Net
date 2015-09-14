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
    internal class BlobContentRepositoryTest
    {
        private Random mRandom;

        [SetUp]
        public virtual void Configure()
        {
            SessionManager.Instance.Configure("test.db");
            Assert.That(SessionManager.Instance.IsConfigured, Is.True);
            this.mRandom = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
        }

        [Test]
        public virtual void CanSave()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = this.GetRandomArray();
                before = repository.RowCount();
                repository.Save(x);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);

                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long id = 0;
            byte[] buffer1 = this.GetRandomArray();

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = buffer1;
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                Assert.That(y, Is.Not.Null);
                byte[] buffer2 = y.Content;
                Assert.That(buffer1, Is.EqualTo(buffer2));
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;
            byte[] buffer1 = this.GetRandomArray();

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = buffer1;
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                buffer1 = this.GetRandomArray();
                y.Content = buffer1;
                repository.SaveOrUpdate(y);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var y = repository.GetById(id);
                byte[] buffer2 = y.Content;
                Assert.That(buffer2, Is.EqualTo(buffer1));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long before = 0;
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                x.Content = this.GetRandomArray();
                before = repository.RowCount();
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                long after = repository.RowCount();
                var y = repository.GetById(id);
                repository.Delete(y);
                after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before));
            }
        }

        private byte[] GetRandomArray()
        {
            int size = this.mRandom.Next(100);
            byte[] buffer = new byte[size];
            this.mRandom.NextBytes(buffer);
            return buffer;
        }
    }
}
