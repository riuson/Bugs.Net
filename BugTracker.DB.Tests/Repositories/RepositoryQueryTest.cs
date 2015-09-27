using BugTracker.DB.Dao;
using BugTracker.DB.Entities;
using BugTracker.DB.Extensions;
using BugTracker.DB.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Tests.Repositories
{
    [TestFixture]
    internal class RepositoryQueryTest
    {
        [Test]
        public void CanFilter()
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                this.CreateItems(session, "filter", 10, 100, 5);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Status> repository = new Repository<Status>(session);

                int count = (from item in repository.Query()
                             where item.Value.Contains("filter")
                             let value = Convert.ToInt32(item.Value.Replace("filter", String.Empty))
                             where value > 50
                             select value).Count();

                Assert.That(count, Is.EqualTo(10));
            }
        }

        [Test]
        public void CanOrderBy()
        {
            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                this.CreateItems(session, "order", 10, 15, 1);

                session.Transaction.Commit();
            }

            using (ISession session = SessionManager.Instance.OpenSession(true))
            {
                IRepository<Status> repository = new Repository<Status>(session);

                {
                    var items = repository.Query()
                        .OrderBy("Value", true);

                    var arr = (from item in items
                               where item.Value.Contains("order")
                               let value = Convert.ToInt32(item.Value.Replace("order", String.Empty))
                               select value).ToArray();

                    Assert.That(arr.ElementAt(0), Is.EqualTo(10));
                    Assert.That(arr.ElementAt(1), Is.EqualTo(11));
                    Assert.That(arr.ElementAt(2), Is.EqualTo(12));
                    Assert.That(arr.ElementAt(3), Is.EqualTo(13));
                    Assert.That(arr.ElementAt(4), Is.EqualTo(14));
                    Assert.That(arr.ElementAt(5), Is.EqualTo(15));
                }

                {
                    var items = repository.Query()
                        .OrderBy("Value", false);

                    var arr = (from item in items
                               where item.Value.Contains("order")
                               let value = Convert.ToInt32(item.Value.Replace("order", String.Empty))
                               select value).ToArray();

                    Assert.That(arr.ElementAt(5), Is.EqualTo(10));
                    Assert.That(arr.ElementAt(4), Is.EqualTo(11));
                    Assert.That(arr.ElementAt(3), Is.EqualTo(12));
                    Assert.That(arr.ElementAt(2), Is.EqualTo(13));
                    Assert.That(arr.ElementAt(1), Is.EqualTo(14));
                    Assert.That(arr.ElementAt(0), Is.EqualTo(15));
                }

            }
        }

        private void CreateItems(ISession session, string prefix, int from, int to, int step)
        {
            IRepository<Status> repository = new Repository<Status>(session);

            for (int i = from; i <= to; i += step)
            {
                var item = new Status();
                item.Value = String.Format("{0}{1}", prefix, i);
                repository.Save(item);
            }
        }
    }
}
