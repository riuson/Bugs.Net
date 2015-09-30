using BugTracker.DB.DataAccess;
using BugTracker.DB.Entities;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.DataAccess.Repositories
{
    [TestFixture]
    internal class InfoRepositoryTest
    {
        [Test]
        public virtual void CanSave()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var x = new Info();
                x.Name = "cansave";
                before = repository.RowCount();
                repository.Save(x);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public virtual void CanSaveOnlyUnique()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var x = new Info();
                x.Name = "cansaveunique";
                before = repository.RowCount();
                repository.Save(x);

                session.Transaction.Commit();
            }

            Assert.That(delegate()
                {
                    using (ISession session = SessionManager.Instance.OpenSession(true))
                    {
                        IRepository<Info> repository = new Repository<Info>(session);
                        var x = new Info();
                        x.Name = "cansaveunique";
                        before = repository.RowCount();
                        repository.Save(x);

                        session.Transaction.Commit();
                    }
                },
                Throws.Exception);
        }

        [Test]
        public virtual void CanGet()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var x = new Info();
                x.Name = "canget";
                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var y = repository.GetById(id);
                Assert.That(y, Is.Not.Null);
            }
        }

        [Test]
        public virtual void CanUpdate()
        {
            long id = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var x = new Info()
                {
                    Name = "canupdate1",
                    Value = "111"
                };

                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var y = repository.GetById(id);
                y.Name = "canupdate2";
                y.Value = "222";

                repository.SaveOrUpdate(y);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var y = repository.GetById(id);
                Assert.That(y.Name, Is.EqualTo("canupdate2"));
                Assert.That(y.Value, Is.EqualTo("222"));
            }
        }

        [Test]
        public virtual void CanDelete()
        {
            long id = 0;
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var x = new Info();
                x.Name = "candelete";
                before = repository.RowCount();
                repository.Save(x);
                id = x.Id;

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                var y = repository.GetById(id);
                repository.Delete(y);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Info> repository = new Repository<Info>(session);
                long after = repository.RowCount();
                Assert.That(after, Is.EqualTo(before));
            }
        }
    }
}
