using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Repositories
{
    [TestFixture]
    internal class RepositoryFilteringTest
    {
        [Test]
        public void CanFilter()
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                this.CreateItems(session, 10, 100, 5);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Status> repository = new Repository<Status>(session);

                int count = repository.Query().Count(item => Convert.ToInt32(item.Value) > 50);

                Assert.That(count, Is.EqualTo(10));
            }
        }

        private void CreateItems(ISession session, int from, int to, int step)
        {
            IRepository<Status> repository = new Repository<Status>(session);

            for (int i = from; i <= to; i += step)
            {
                var item = new Status();
                item.Value = i.ToString();
                repository.Save(item);
            }
        }
    }
}
