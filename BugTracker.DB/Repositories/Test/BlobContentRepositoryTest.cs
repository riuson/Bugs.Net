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
    internal class BlobContentRepositoryTest
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
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                byte[] buffer = this.GetRandomArray();

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    x.ReadFrom(ms);
                }

                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);
            }
        }

        [Test]
        public virtual void CanGet()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                byte[] buffer1 = this.GetRandomArray();

                using (MemoryStream ms = new MemoryStream(buffer1))
                {
                    x.ReadFrom(ms);
                }

                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                Assert.IsNotNull(y);
                byte[] buffer2;

                using (MemoryStream ms = new MemoryStream())
                {
                    x.WriteTo(ms);
                    buffer2 = ms.GetBuffer();
                }

                Assert.IsTrue(this.CompareBuffers(buffer1, buffer2));
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                byte[] buffer1 = this.GetRandomArray();

                using (MemoryStream ms = new MemoryStream(buffer1))
                {
                    x.ReadFrom(ms);
                }

                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                buffer1 = this.GetRandomArray();

                using (MemoryStream ms = new MemoryStream(buffer1))
                {
                    y.ReadFrom(ms);
                }

                repository.SaveOrUpdate(y);

                after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                y = repository.GetById(x.Id);
                byte[] buffer2;

                using (MemoryStream ms = new MemoryStream())
                {
                    y.WriteTo(ms);
                    buffer2 = ms.GetBuffer();
                }

                Assert.IsTrue(this.CompareBuffers(buffer1, buffer2));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<BlobContent> repository = new Repository<BlobContent>(session);
                var x = new BlobContent();
                byte[] buffer = this.GetRandomArray();

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    x.ReadFrom(ms);
                }

                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                repository.Delete(x);
                after = repository.RowCount();
                Assert.AreEqual(before, after);
            }
        }

        private byte[] GetRandomArray()
        {
            Random rnd = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
            int size = rnd.Next(10000);
            byte[] buffer = new byte[size];
            rnd.NextBytes(buffer);
            return buffer;
        }

        private bool CompareBuffers(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1.Length != buffer2.Length)
            {
                return false;
            }

            for (int i = 0; i < buffer1.Length; i++)
            {
                if (buffer1[i] != buffer2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
