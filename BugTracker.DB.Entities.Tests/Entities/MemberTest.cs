using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BugTracker.DB.Entities.Tests
{
    [TestFixture]
    internal class MemberTest
    {
        [Test]
        public void Operation()
        {
            var member = new Member
            {
                FirstName = "First",
                LastName = "Last",
                EMail = "Email"
            };

            Assert.AreEqual(member.FirstName, "First");
            Assert.AreEqual(member.LastName, "Last");
            Assert.AreEqual(member.EMail, "Email");
            Assert.AreEqual(member.GetFullName(), "First Last");
        }
    }
}
