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
    internal class PriorityRepositoryTest
    {
        [SetUp]
        public void Configure()
        {
            SessionManager.Instance.Configure("test.db");
            Assert.IsTrue(SessionManager.Instance.IsConfigured);
        }

        [Test]
        public void CanSave()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                var x = new Priority();
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);
            }
        }

        [Test]
        public void CanGet()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                var x = new Priority();
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                Assert.IsNotNull(y);
            }
        }

        [Test]
        public void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                var x = new Priority();
                x.Value = "Test1";
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                y.Value = "Test2";
                repository.SaveOrUpdate(y);
                after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                y = repository.GetById(x.Id);
                Assert.AreEqual(x, y);
                Assert.AreSame(x, y);
                Assert.AreEqual(x.Value, y.Value);
            }
        }

        [Test]
        public void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                var x = new Priority();
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                repository.Delete(x);
                after = repository.RowCount();
                Assert.AreEqual(before, after);
            }
        }
    }
}
