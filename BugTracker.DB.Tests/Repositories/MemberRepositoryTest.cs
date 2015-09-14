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
    internal class MemberRepositoryTest
    {
        [Test]
        public virtual void CanSave()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member();
                before = repository.RowCount();
                repository.Save(x);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public virtual void CanGet()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member();
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var y = repository.GetById(id);
                Assert.That(y, Is.Not.Null);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member()
                {
                    FirstName = "First",
                    LastName = "Last",
                    EMail = "Email"
                };

                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var y = repository.GetById(id);
                y.FirstName = "First2";
                y.LastName = "Last2";
                y.EMail = "Email2";

                repository.SaveOrUpdate(y);
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var y = repository.GetById(id);
                Assert.That(y.FirstName, Is.EqualTo("First2"));
                Assert.That(y.LastName, Is.EqualTo("Last2"));
                Assert.That(y.EMail, Is.EqualTo("Email2"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var x = new Member();
                before = repository.RowCount();
                repository.Save(x);
                id = x.Id;
            }

            using (ISession session = SessionManager.Instance.OpenSession())
            {
                IRepository<Member> repository = new Repository<Member>(session);
                var y = repository.GetById(id);
                repository.Delete(y);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before));
            }
        }
    }
}
