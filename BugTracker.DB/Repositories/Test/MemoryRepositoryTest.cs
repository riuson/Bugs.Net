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
    internal class MemoryRepositoryTest
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
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member();
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
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };
                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                Assert.IsNotNull(y);

                Assert.AreEqual(x.FirstName, y.FirstName);
                Assert.AreEqual(x.LastName, y.LastName);
                Assert.AreEqual(x.EMail, y.EMail);
                Assert.AreEqual(x, y);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };

                long before = repository.RowCount();
                repository.Save(x);
                long after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                var y = repository.GetById(x.Id);
                y.FirstName = "First2";
                y.LastName = "Last2";
                y.EMail = "Email2";

                repository.SaveOrUpdate(y);

                after = repository.RowCount();
                Assert.AreEqual(before + 1, after);

                y = repository.GetById(x.Id);
                Assert.AreEqual(y.FirstName, "First2");
                Assert.AreEqual(y.LastName, "Last2");
                Assert.AreEqual(y.EMail, "Email2");
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member();
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
