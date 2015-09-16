using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Classes
{
    [TestFixture]
    internal class TransactionTest
    {
        [Test]
        public void CanWriteWithTransaction()
        {
            long before = 0;

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                before = repository.RowCount();

                repository.Save(new Priority());

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                long after = repository.RowCount();

                Assert.That(after, Is.EqualTo(before + 1));
            }
        }

        [Test]
        public void CannotWriteWithoutTransaction()
        {
            long before = 0;

            Assert.That(delegate()
                {
                    using (ISession session = SessionManager.Instance.OpenSession(false))
                    {
                        IRepository<Priority> repository = new Repository<Priority>(session);
                        before = repository.RowCount();

                        repository.Save(new Priority());
                    }
                },
                Throws.Exception.TypeOf<InvalidOperationException>());

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                long after = repository.RowCount();

                Assert.That(after, Is.EqualTo(before));
            }
        }

        [Test]
        public void CanRollbackOnException()
        {
            long before = 0;

            try
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Priority> repository = new Repository<Priority>(session);
                    before = repository.RowCount();

                    repository.Save(new Priority());
                    repository.Save(new Priority());
                    repository.Save(null);

                    session.Transaction.Commit();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                long after = repository.RowCount();

                Assert.That(after, Is.EqualTo(before));
            }
        }

        [Test]
        public void CanRollbackWithTransaction()
        {
            long before = 0;

            try
            {
                using (ISession session = SessionManager.Instance.OpenSession(true))
                {
                    IRepository<Priority> repository = new Repository<Priority>(session);
                    before = repository.RowCount();

                    repository.Save(new Priority());
                    repository.Save(new Priority());
                    repository.Save(null);

                    session.Transaction.Rollback();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            using (ISession session = SessionManager.Instance.OpenSession(false))
            {
                IRepository<Priority> repository = new Repository<Priority>(session);
                long after = repository.RowCount();

                Assert.That(after, Is.EqualTo(before));
            }
        }
    }
}
