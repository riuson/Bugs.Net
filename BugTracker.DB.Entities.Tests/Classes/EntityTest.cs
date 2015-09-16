using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class EntityTest
    {
        [Test]
        public void Operation()
        {
            long id1 = 12345678;
            var entity1 = new EntityEx(id1);
            Assert.That(id1, Is.EqualTo(entity1.Id));

            var entity2 = new EntityEx(id1);
            Assert.That(id1, Is.EqualTo(entity2.Id));

            Assert.That(entity2, Is.Not.SameAs(entity1));
            Assert.That(entity2, Is.EqualTo(entity1));
            Assert.That(entity1 == entity2, Is.True);
            Assert.That(entity1 != entity2, Is.False);
            Assert.That(entity1.Equals(entity2), Is.True);

            long id2 = 87654321;
            entity2 = new EntityEx(id2);
            Assert.That(id1, Is.Not.EqualTo(entity2.Id));
            Assert.That(id2, Is.EqualTo(entity2.Id));

            Assert.That(entity2, Is.Not.SameAs(entity1));
            Assert.That(entity2, Is.Not.EqualTo(entity1));
            Assert.That(entity1 == entity2, Is.False);
            Assert.That(entity1 != entity2, Is.True);
            Assert.That(entity1.Equals(entity2), Is.False);
        }

        private class EntityEx : Entity<long>
        {
            public EntityEx(long id)
            {
                this.Id = id;
            }
        }
    }
}
