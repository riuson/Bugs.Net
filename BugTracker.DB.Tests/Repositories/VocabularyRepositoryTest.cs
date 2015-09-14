using BugTracker.DB.Classes;
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
            Assert.That(SessionManager.Instance.IsConfigured, Is.True);
        }

        [Test]
        public virtual void CanSave()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
                before = repository.RowCount();
                repository.Save(x);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long ticketId = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var x = new T();
                repository.Save(x);
                ticketId = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var y = repository.GetById(ticketId);
                Assert.IsNotNull(y);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long ticketId = 0;
            var x = new T();

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                x.Value = "Test1";
                repository.Save(x);
                ticketId = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var y = repository.GetById(ticketId);
                y.Value = "Test2";
                repository.SaveOrUpdate(y);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                var y = repository.GetById(ticketId);
                Assert.That(x, Is.EqualTo(y));
                Assert.That(y.Value, Is.EqualTo("Test2"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                before = repository.RowCount();
                var x = new T();
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<T> repository = new Repository<T>(session);
                T x = repository.GetById(id);
                repository.Delete(x);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before));
            }
        }
    }
}
