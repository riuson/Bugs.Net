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
            Assert.AreEqual(entity1.Id, id1);

            var entity2 = new EntityEx(id1);
            Assert.AreEqual(entity2.Id, id1);

            Assert.AreNotSame(entity1, entity2);
            Assert.AreEqual(entity1, entity2);
            Assert.IsTrue(entity1 == entity2);
            Assert.IsFalse(entity1 != entity2);
            Assert.IsTrue(entity1.Equals(entity2));

            long id2 = 87654321;
            entity2 = new EntityEx(id2);
            Assert.AreNotEqual(entity2.Id, id1);
            Assert.AreEqual(entity2.Id, id2);

            Assert.AreNotSame(entity1, entity2);
            Assert.AreNotEqual(entity1, entity2);
            Assert.IsFalse(entity1 == entity2);
            Assert.IsTrue(entity1 != entity2);
            Assert.IsFalse(entity1.Equals(entity2));
        } 

        private class EntityEx : Entity
        {
            public EntityEx(long id)
            {
                this.Id = id;
            }
        }
    }
}
