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
    [TestFixture(typeof(Priority))]
    [TestFixture(typeof(ProblemType))]
    [TestFixture(typeof(Solution))]
    [TestFixture(typeof(Status))]
    internal class VocabularyRepositoryTest<T> where T : DB.Entities.Entity, IVocabulary, new()
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
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
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
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                Assert.IsNotNull(y);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
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
        public virtual void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
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
